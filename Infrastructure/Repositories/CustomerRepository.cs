using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Requests;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;


//una interface es un contrato que la clase debe cumplir
namespace Infrastructure.Repositories;
public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerRepository(ApplicationDbContext context)
    {
     
        _context = context;
    }

    public async Task<CustomerDTO> Add(string firstName, string? lastName)
    {
        var customerToCreate = new Customer
        {
            FirstName = firstName,
            LastName = lastName
        };

        _context.Customers.Add(customerToCreate); //aqui no impactamos aun la BD
        await _context.SaveChangesAsync(); //esto impacta en la BD

        return new CustomerDTO
        {
            Id = customerToCreate.Id,
            FullName = $"{customerToCreate.FirstName} {customerToCreate.LastName}",
            Phone = customerToCreate.Phone,
            Email = customerToCreate.Email,
            BirthDate = customerToCreate.BirthDate
        };

    }

    public async Task<List<CustomerDTO>> List(PaginationRequest request)
    {
        //        int currentPage = page ?? 1;
        //int currentPageSize = pageSize ?? 10;

        var customersDto = await _context.Customers
            .Skip((request.Page.Value - 1) * request.PageSize.Value)
            .Take(request.PageSize.Value)
            .Select(customer => new CustomerDTO
            {
                Id = customer.Id,
                FullName = $"{customer.FirstName} {customer.LastName}",
                Phone = customer.Phone,
                Email = customer.Email,
                BirthDate = customer.BirthDate
            })
            .ToListAsync();

        return customersDto;

    }

    public async Task<CustomerDTO> Update(int id, string firstName, string? lastName)
    {
        var entity = await VerifyExists(id);

        entity.FirstName = firstName;
        entity.LastName = lastName;
        _context.Customers.Update(entity);
        await _context.SaveChangesAsync();

        return new CustomerDTO
        {
            Id = id,
            FullName = $"{entity.FirstName} {entity.LastName}",
            Phone = entity.Phone,
            Email = entity.Email,
            BirthDate = entity.BirthDate
        };


    }
    public async Task<bool> Delete(int id)
    {
        var entity = await VerifyExists(id);
        _context.Customers.Remove(entity);
       var result =  await _context.SaveChangesAsync();

        return result > 0;
    }

    public async Task<CustomerDTO> Get(int id)
    {
        var entity = await VerifyExists(id);
        return new CustomerDTO
        {
            Id = id,
            FullName = $"{entity.FirstName} {entity.LastName}",
            Phone = entity.Phone,
            Email = entity.Email,
            BirthDate = entity.BirthDate
        };
    }

    private async Task<Customer> VerifyExists(int id)
    {
        var entity = await _context.Customers.FindAsync(id);
        if (entity == null) throw new Exception("No se encontro con el id solicitado");
        return entity;
    }
}

