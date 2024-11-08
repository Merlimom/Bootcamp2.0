using Core.DTOs;
using Core.Entities;
using Core.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//una interface es un contrato que la clase debe cumplir

namespace Core.Interfaces.Repositories;
public interface ICustomerRepository
{
    Task<List<CustomerDTO>> List(PaginationRequest request);
    Task<CustomerDTO> Get(int id);
    Task<CustomerDTO> Add(string firstName, string? lastName);
    Task<CustomerDTO> Update(int id, string firstName, string? lastName);
    Task <bool> Delete(int id);
}
