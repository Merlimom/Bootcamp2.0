﻿using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Requests;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class CustomerController : BaseApiController
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    [HttpGet("List")]
    public async Task<IActionResult> List([FromQuery] PaginationRequest request)

    {
        //return Ok(await _customerRepository.List());
        int page = request.Page ?? 1;      // Predeterminado a 1 si no se proporciona
        int pageSize = request.PageSize ?? 10;  // Predeterminado a 10 si no se proporciona

        var response = await _customerRepository.List(page, pageSize);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        return Ok(await _customerRepository.Get(id));
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] Customer customer)
    {
        return Ok(await _customerRepository.Add(customer.FirstName, customer.LastName));
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] Customer customer)
    {
        return Ok(await _customerRepository.Update(customer.Id, customer.FirstName, customer.LastName));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        return Ok(await _customerRepository.Delete(id));
    }
}