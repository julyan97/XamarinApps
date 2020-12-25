using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GreenDo.Services
{
    public interface ICameraService
    {
        Task<MediaFile> TakeVideoAsync();
    }
}
