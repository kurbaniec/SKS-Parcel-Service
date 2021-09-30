﻿using System;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces
{
    public interface ILogisticsPartnerLogic
    {
        /// <summary>
        /// Submit a new parcel to the logistics service. 
        /// </summary>
        /// <param name="body"></param>
        /// <param name="trackingId"></param>
        /// <returns></returns>
        public Parcel TransitionParcel(Parcel? body, string? trackingId);
    }
}