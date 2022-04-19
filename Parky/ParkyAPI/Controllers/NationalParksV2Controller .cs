
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.IRepository.Repository;
using ParkyAPI.Models;
using ParkyAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkyAPI.Controllers
{
    [Route("api/v{version:apiVersion}/nationalparks")]
    [ApiVersion("2.0")]
    //[Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
    [ProducesResponseType(400)]
    public class NationalParksV2Controller : Controller
    {
        private readonly INationalParkRepository _npRepo;
        private readonly IMapper _mapper;
        public NationalParksV2Controller(INationalParkRepository npRepo, IMapper mapper)
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
            var obj = _npRepo.GetNationalPark().FirstOrDefault();
       
            return Ok(_mapper.Map<NationalParkDto>(obj));
        }    
      
    }

}

