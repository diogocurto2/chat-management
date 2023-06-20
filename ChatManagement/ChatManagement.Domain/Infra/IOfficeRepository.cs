using ChatManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatManagement.Domain.Infra
{
    public interface IOfficeRepository
    {

        Task<Office> Get();

        Task Save(Office office);
    }
}
