using Core.DTOs;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//una interface es un contrato que la clase debe cumplir

namespace Core.Interfaces.Repositories;
public interface ICustomerRepository
{
    Task<List<CustomerDTO>> List(int? page = 1, int? pageSize = 10);
    Task<CustomerDTO> Get(int id);
    Task<List<CustomerDTO>> Add(string firstName, string? lastName);
    Task<List<CustomerDTO>> Update(int id, string firstName, string? lastName);
    Task<List<CustomerDTO>> Delete(int id);
}
