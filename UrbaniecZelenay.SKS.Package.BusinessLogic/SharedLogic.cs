using System.Collections.Generic;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;
using DalHop = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Hop;
using DalTruck = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Truck;
using DalTransferwarehouse = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Transferwarehouse;
using DalHopArrival = UrbaniecZelenay.SKS.Package.DataAccess.Entities.HopArrival;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic
{
    public class SharedLogic
    {
        public static List<HopArrival>? PredictFutureHops(Hop recipientStartHop, Hop senderStartHop)
        {
            var currentRecipientHop = recipientStartHop;
            var currentSenderHop = senderStartHop;
            var recipientFutureHops = new List<HopArrival>();
            var senderFutureHops = new List<HopArrival>();
            // Build Future Hops hierarchy
            while (currentRecipientHop?.Code != currentSenderHop?.Code &&
                   currentSenderHop != null && currentRecipientHop != null)
            {
                recipientFutureHops.Add(new HopArrival
                {
                    Hop = currentRecipientHop
                });
                currentRecipientHop = currentRecipientHop.PreviousHop?.Hop;
                senderFutureHops.Add(new HopArrival
                {
                    Hop = currentSenderHop
                });
                currentSenderHop = currentSenderHop.PreviousHop?.Hop;
            }

            // Check if Route was found
            if (currentSenderHop == null || currentRecipientHop == null)
            {
                return null;
            }

            // Build one List
            // Note: senderFutureHops will contain afterwards the full Future Hops List
            senderFutureHops.Add(new HopArrival
            {
                Hop = currentSenderHop
            });
            recipientFutureHops.Reverse();
            senderFutureHops.AddRange(recipientFutureHops);
            return senderFutureHops;
        }
    }
}