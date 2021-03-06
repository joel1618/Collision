pc.script.attribute('heightMap', 'asset', [], {
    max: 1,
    type: 'texture'
});
pc.script.attribute('minHeight', 'number', 0);
pc.script.attribute('maxHeight', 'number', 10);
pc.script.attribute('width', 'number', 100);
pc.script.attribute('depth', 'number', 100);
pc.script.attribute('subdivisions', 'number', 250);
pc.script.attribute('material', 'asset', [], {
    max: 1,
    type: 'material'
});

pc.script.create('terrain', function (app) {

    // Creates a new Terrain instance
    var Terrain = function (entity) {
        this.entity = entity;
    };

    Terrain.prototype = {
        createTerrainVertexData: function (options) {
            var positions = [];
            var uvs = [];
            var indices = [];
            var row, col;

            for (row = 0; row <= options.subdivisions; row++) {
                for (col = 0; col <= options.subdivisions; col++) {
                    var position = new pc.Vec3((col * options.width) / options.subdivisions - (options.width / 2.0), 0, ((options.subdivisions - row) * options.height) / options.subdivisions - (options.height / 2.0));

                    var heightMapX = (((position.x + options.width / 2) / options.width) * (options.bufferWidth - 1)) | 0;
                    var heightMapY = ((1.0 - (position.z + options.height / 2) / options.height) * (options.bufferHeight - 1)) | 0;

                    var pos = (heightMapX + heightMapY * options.bufferWidth) * 4;
                    var r = options.buffer[pos] / 255.0;
                    var g = options.buffer[pos + 1] / 255.0;
                    var b = options.buffer[pos + 2] / 255.0;

                    var gradient = r * 0.3 + g * 0.59 + b * 0.11;

                    position.y = options.minHeight + (options.maxHeight - options.minHeight) * gradient;

                    positions.push(position.x, position.y, position.z);
                    uvs.push(col / options.subdivisions, 1.0 - row / options.subdivisions);
                }
            }

            for (row = 0; row < options.subdivisions; row++) {
                for (col = 0; col < options.subdivisions; col++) {
                    indices.push(col + row * (options.subdivisions + 1));
                    indices.push(col + 1 + row * (options.subdivisions + 1));
                    indices.push(col + 1 + (row + 1) * (options.subdivisions + 1));

                    indices.push(col + row * (options.subdivisions + 1));
                    indices.push(col + 1 + (row + 1) * (options.subdivisions + 1));
                    indices.push(col + (row + 1) * (options.subdivisions + 1));
                }
            }

            var normals = pc.calculateNormals(positions, indices);

            return {
                indices: indices,
                positions: positions,
                normals: normals,
                uvs: uvs
            };
        },
        
        createTerrainFromHeightMap: function (img, subdivisions) {
            var canvas = document.createElement("canvas");
            var context = canvas.getContext("2d");
            var bufferWidth = img.width;
            var bufferHeight = img.height;
            canvas.width = bufferWidth;
            canvas.height = bufferHeight;

            context.drawImage(img, 0, 0);

            var buffer = context.getImageData(0, 0, bufferWidth, bufferHeight).data;
            var vertexData = this.createTerrainVertexData({
                width: this.width,
                height: this.depth,
                subdivisions: subdivisions,
                minHeight: this.minHeight,
                maxHeight: this.maxHeight,
                buffer: buffer,
                bufferWidth: bufferWidth,
                bufferHeight: bufferHeight
            });

            var node = new pc.GraphNode();
            var material = app.assets.get(this.material).resource;

            var mesh = pc.createMesh(app.graphicsDevice, vertexData.positions, {
                normals: vertexData.normals,
                uvs: vertexData.uvs,
                indices: vertexData.indices
            });

            var meshInstance = new pc.MeshInstance(node, mesh, material);

            var model = new pc.Model();
            model.graph = node;
            //disable the floor
            //model.meshInstances.push(meshInstance);
            
            return model;
        },
    
        // Called once after all resources are loaded and before the first update
        initialize: function () {
            var img = app.assets.get(this.heightMap).resource.getSource();
            var renderModel = this.createTerrainFromHeightMap(img, this.subdivisions);
            var collisionModel = this.createTerrainFromHeightMap(img, this.subdivisions / 2);

            if(this.entity.model == null){
                this.entity.addComponent('model');
                this.entity.model.model = renderModel;
            }
            if (this.entity.collision == null) {
                this.entity.addComponent('collision', {
                    type: 'mesh'
                });
                this.entity.collision.model = collisionModel;
            }
            if (this.entity.rigidbody == null) {
                this.entity.addComponent('rigidbody', {
                    friction: 0.5,
                    type: 'static'
                });
            }

            // HACK: This line is non-API but it is currently required to set a procedurally created
            //model onto a collision component
            if(app.systems.collision.implementations.mesh !== undefined)
                app.systems.collision.implementations.mesh.doRecreatePhysicalShape(this.entity.collision);
        },

        // Called every frame, dt is time in seconds since last update
        update: function (dt) {
        }
    };

    return Terrain;
});