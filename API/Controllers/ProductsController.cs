using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Specifications;
using Model.DTOs;
using Repository;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepo<Product> _productsRepo;
        private readonly IGenericRepo<ProductBrand> _brandsRepo;
        private readonly IGenericRepo<ProductType> _typesRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepo<Product> productsRepo, 
            IGenericRepo<ProductBrand> brandsRepo, 
            IGenericRepo<ProductType> typesRepo,
            IMapper mapper)
        {
            _productsRepo = productsRepo;
            _brandsRepo = brandsRepo;
            _typesRepo = typesRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetAllProducts()
        {
            var products = await _productsRepo.GetAllAsyncWithSpec(new ProductsWithTypesAndBrandsSpecification());

            return Ok(products.Select(product => _mapper.Map<ProductToReturnDto>(product)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProductById(int id)
        {
            var product = await _productsRepo.GetEntityWithSpec(new ProductsWithTypesAndBrandsSpecification(id));
            if (product == null) return NotFound();
            return _mapper.Map<ProductToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands(){
            var productBrands = await _brandsRepo.GetAllAsync();
            return Ok(productBrands);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            var productTypes = await _typesRepo.GetAllAsync();
            return Ok(productTypes);
        }
    }
}
