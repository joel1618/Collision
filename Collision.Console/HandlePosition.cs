using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Configuration;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Practices.Unity;
using Collision.Data.Repositories.Interfaces;
using Collision.Data.Repositories;
using Collision.Core.Models;
using Newtonsoft.Json;
using Collision.Business.Interfaces;

namespace Collision.Console
{
    public class HandlePosition
    {

        private IPositionRepository positionRepository;
        private IAircraftRepository aircraftRepository;
        private IConflictRepository conflictRepository;
        private IPositionService positionService;
        private HandleCollision collision = null;

        //TODO: Figure out why not executing quickly
        public HandlePosition(IPositionRepository positionRepository, IAircraftRepository aircraftRepository, IConflictRepository conflictRepository, IPositionService positionService)
        {
            this.positionRepository = positionRepository;
            this.aircraftRepository = aircraftRepository;
            this.conflictRepository = conflictRepository;
            this.positionService = positionService;
        }

        public void HandlePositions(List<Aircraft> aircrafts)
        {
            do
            {
                foreach (Aircraft aircraft in aircrafts)
                {
                    if (aircraft.IsActive)
                    {
                        System.Console.WriteLine("Handling position for " + aircraft.CarrierName + " flight " + aircraft.FlightNumber);
                        var position = _positionRepository.Search(e => e.AircraftId == aircraft.Id, 0, 1).FirstOrDefault();
                        if (position == null)
                        {
                            //No position yet exists for this aircraft and we need to create a new one
                            position = new Position();
                            position.Aircraft = aircraft;
                            //Call api for flight
                            if (positionService.EvaluateFlightInformation(position))
                            {
                                //Create position in database
                                position = positionRepository.Create(position);
                                //Calculate the positions bounds
                                HandleBoundingBox.CalculateBoundingBox(position);
                                //Update position object in database
                                position = positionRepository.Update(position.Id, position);
                                //Call HandleCollisions to start evaluating this position for potential collisions
                                HandleCollision(aircraft, position);
                            }
                            else
                            {
                                //There was an error from the API
                            }
                        }
                        else
                        {
                            //We found a position and need to update position from api and recalculate boundingbox
                            if (positionService.EvaluateFlightInformation(position))
                            {
                                //Calculate the positions bounds
                                HandleBoundingBox.CalculateBoundingBox(position);
                                //Update position object in database
                                position = positionRepository.Update(position.Id, position);
                                //Call HandleCollisions to start evaluating this position for potential collisions
                                HandleCollision(aircraft, position);
                            }
                            else
                            {
                                //There was an error from the API
                            }
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("Handling inactive aircraft " + aircraft.CarrierName + " flight " + aircraft.FlightNumber);
                        HandleInActiveAircraft(aircraft);
                    }
                }
                Thread.Sleep(10000);
            } while (true);
        }

        public void HandleCollision(Aircraft aircraft, Position position)
        {
            if (collision == null)
            {
                collision = new HandleCollision(positionRepository, conflictRepository);
            }
            collision.HandleCollisions(position);
        }

        private void HandleInActiveAircraft(Aircraft aircraft)
        {
            var position = positionRepository.Search(e => e.AircraftId == aircraft.Id, 0, 1).FirstOrDefault();
            positionService.NullifyPosition(position);
            //Remove collision potentials associated with this position
            RemoveCollisions(position);
            position.IsActive = false; position.IsInFlight = false;
            positionRepository.Update(position.Id, position);
                }

        private void RemoveCollisions(Position position)
        {
            var collisions = _conflictRepository.Search(e => e.PositionId1 == position.Id, 0, 100);
            foreach (var collision in collisions)
            {
                conflictRepository.Delete(collision.Id);
        }
        }
    }
}
