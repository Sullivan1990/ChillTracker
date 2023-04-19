using ChilliTracker.Business.Interfaces;
using ChilliTracker.Data.DataModels;
using ChilliTracker.Data.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Numerics;
using System.Security.Cryptography;

namespace ChilliTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChilliPlantController : ControllerBase
    {
        private readonly IChilliPlantRepository _chilliRepo;
        public ChilliPlantController(IChilliPlantRepository chilliRepo)
        {
            _chilliRepo = chilliRepo;
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateChilli(ChilliPlantCreateNewDTO newPlant)
        {
            if (newPlant == null) { return BadRequest("No Chilli Plant Data received"); }

            var userId = GetUserIDFromClaim();

            if (String.IsNullOrEmpty(userId)) return Unauthorized();

            _chilliRepo.AddNewChilli(newPlant, userId);
            return CreatedAtAction(nameof(CreateChilli), null);

        }

        [Authorize]
        [HttpPost("AddPottingEventToChilli/{chilliPlantId}")]
        public IActionResult AddPottingEventToChilli([FromRoute]string chilliPlantId, [FromBody]PlantPottingEventDTO pottingEvent)
        {
            if (pottingEvent == null) { return BadRequest("No Chilli Plant Data received"); }

            var userId = GetUserIDFromClaim();

            if (String.IsNullOrEmpty(userId)) return Unauthorized();

            PlantPottingEvent newPottingEvent = new PlantPottingEvent
            {
                PottingEventID = ObjectId.GenerateNewId().ToString(),
                Date = pottingEvent.Date,
                Conditions = pottingEvent.Conditions,
                Container = pottingEvent.Container,
                Medium = pottingEvent.Medium,
                Notes = pottingEvent.Notes
            };

            _chilliRepo.AddPottingEventToChilli(chilliPlantId, newPottingEvent, userId);

            return CreatedAtAction("AddPottingEventToChilli", newPottingEvent);
        }

        [Authorize]
        [HttpGet("GetAllPlants")]
        public IActionResult GetAllChilliPlantsForUser()
        {
            string userId = GetUserIDFromClaim();
            if (String.IsNullOrEmpty(userId)) return Unauthorized();

            var chillies = _chilliRepo.GetAllForUser(userId);

            var chilliReturnList = chillies.Select(c => new ChilliPlantReturnDTO
            {
                _id = c._id.ToString(),
                Identifier = c.Identifier,
                Species = c.Species,
                Variety = c.Variety,
                Planted = c.Planted,
                Germinated = c.Germinated,
                FirstHarvest = c.FirstHarvest,
                IsHealthy = c.IsHealthy,
                IsGerminated = c.IsGerminated,
                HarvestEvents = c.HarvestEvents,
                PlantIssues = c.PlantIssues,
                PlantPottingEvents = c.PlantPottingEvents
            }).AsEnumerable();

            return Ok(chilliReturnList);
        }

        private string GetUserIDFromClaim()
        {
            var userIdClaim = HttpContext?.User?.Claims?.Where(c => c.Type == "Id").FirstOrDefault();

            return userIdClaim?.Value ?? string.Empty;
        }

    }
}
