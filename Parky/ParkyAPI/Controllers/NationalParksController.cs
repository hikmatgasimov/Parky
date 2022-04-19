
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.IRepository.Repository;
using ParkyAPI.Models;
using ParkyAPI.Repository;
using System;
using System.Collections.Generic;

namespace ParkyAPI.Controllers
{
    [Route("api/v{version:apiVersion}/nationalparks")]
    // [Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
    [ProducesResponseType(400)]
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _npRepo;
        private readonly IMapper _mapper;
        public NationalParksController(INationalParkRepository npRepo, IMapper mapper)
        {
            _npRepo = npRepo ?? throw new ArgumentNullException(nameof(npRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        /// <summary>
        /// Get list of national parks
        /// </summary>
        /// <returns></returns>


        [HttpGet]
        [ProducesResponseType(200,Type=typeof(List<NationalPark>))]
       
        public IActionResult GetNationalPark()
        {
            var objList = _npRepo.GetNationalPark();

            var objDto = new List<NationalParkDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<NationalParkDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Get individual national park
        /// </summary>
        /// <param name="nationalParkId"> The Id of individual the national Park</param>
        /// <returns></returns>
        [HttpGet("{nationalParkId:int}", Name = " GetNationalPark")]
        [ProducesResponseType(200, Type = typeof(NationalPark))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var obj = _npRepo.GetNationalPark(nationalParkId);

            if (obj == null) return NotFound();

            var objDto = _mapper.Map<NationalParkDto>(obj);
            //var objDto = new NationalParkDto()
            //{
            //    Created = obj.Created,
            //    Id = obj.Id,
            //    Name = obj.Name,
            //    State = obj.State,
            //};

            return Ok(objDto);
        }
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(NationalPark))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
      
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null) return BadRequest(); ;

            if (_npRepo.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National Park Exists");
                return StatusCode(404, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);

            if (!_npRepo.CreateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtAction("GetNationalPark", new {version=HttpContext.GetRequestedApiVersion().ToString() , nationalParkId = nationalParkObj.Id }, nationalParkObj);
        }
        [HttpPatch("{id:int}", Name = "UpdateNationalPark")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateNationalPark(int id,[FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null|| id != nationalParkDto.Id) return BadRequest(ModelState); ;
        
            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);

            if (!_npRepo.UpdateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();           
        }
        [HttpDelete("{id:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public IActionResult DeleteNationalPark(int id)
        {     
            if (_npRepo.NationalParkExists(id))
            {
                return NotFound();
            }
            var obj = _npRepo.GetNationalPark(id);

            if (!_npRepo.DeleteNationalPark(obj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {obj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }

}

