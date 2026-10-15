using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace Ticketing.Infrastructure.FileService.Contracts
{

    public interface IFileService
    {
        public string? Upload(IFormFile file, string folder);
        public void Delete(string fileName);
    }
}
