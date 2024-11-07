using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Repositories;
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
  
    public async Task<List<CustomerDTO>> Add(string firstName, string? lastName)
    {
        var entity = new Customer
        {
            FirstName = firstName,
            LastName = lastName
        };

        _context.Customers.Add(entity); //aqui no impactamos aun la BD
        await _context.SaveChangesAsync(); //esto impacta en la BD

        return await List();
    }

    public async Task<List<CustomerDTO>> List(int? page = 1, int? pageSize = 10)
    {
        //var entities = await _context.Customers.ToListAsync();
        //var dtos = entities.Select(customer => new CustomerDTO
        //{
        //    Id = customer.Id,
        //    FullName = $"{customer.FirstName} {customer.LastName}",
        //    Phone = customer.Phone,
        //    Email = customer.Email,
        //    BirthDate = customer.BirthDate,
        //});
        //return dtos.ToList();
        int currentPage = page ?? 1;
        int currentPageSize = pageSize ?? 10;

        // Aplicamos Skip y Take según la página y el tamaño de página
        var customers = await _context.Customers
            .Skip((currentPage - 1) * currentPageSize)
            .Take(currentPageSize)
            .Select(customer => new CustomerDTO
            {
                Id = customer.Id,
                FullName = $"{customer.FirstName} {customer.LastName}",
                Phone = customer.Phone,
                Email = customer.Email,
                BirthDate = customer.BirthDate
            })
            .ToListAsync();

        return customers;

    }

    public async Task<List<CustomerDTO>> Update(int id, string firstName, string? lastName)
    {
        var entity = await VerifyExists(id);
        entity.FirstName = firstName;
        entity.LastName = lastName;
        _context.Customers.Update(entity);
        await _context.SaveChangesAsync();
        return await List();
    }
    public async Task<List<CustomerDTO>>Delete(int id)
    {
        var entity = await VerifyExists(id);
        _context.Customers.Remove(entity);
        await _context.SaveChangesAsync();
        return await List();
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

