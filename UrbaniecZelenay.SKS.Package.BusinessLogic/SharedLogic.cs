using System.Collections.Generic;
using DalHop = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Hop;
using DalTruck = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Truck;
using DalTransferwarehouse = UrbaniecZelenay.SKS.Package.DataAccess.Entities.Transferwarehouse;
using DalHopArrival = UrbaniecZelenay.SKS.Package.DataAccess.Entities.HopArrival;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic
{
    public class SharedLogic
    {
        public static List<DalHopArrival>? PredictFutureHops(DalHop recipientStartHop, DalHop senderStartHop)
        {
            var currentRecipientHop = recipientStartHop;
            var currentSenderHop = senderStartHop;
            var recipientFutureHops = new List<DalHopArrival>();
            var senderFutureHops = new List<DalHopArrival>();
            // Note: Misconception, Trucks and Transferwarehouses are on the same level
            /*// If recipient is a Truck add one previous Hop to the current sender Hop
            // Why? Hop hierarchy is a balanced tree and trucks are one hierarchy level lower than
            // Transferwarehouses (which belong to the same level as normal Warehouses which have trucks)
            if (currentRecipientHop is DalTruck)
            {
                recipientFutureHops.Add(new DalHopArrival
                {
                    Hop = currentRecipientHop
                });
                currentRecipientHop = currentRecipientHop.PreviousHop;
            }

            senderFutureHops.Add(new DalHopArrival
            {
                Hop = currentSenderHop
            });
            currentSenderHop = currentSenderHop.PreviousHop;*/
            // Build Future Hops hierarchy
            while (currentRecipientHop != currentSenderHop &&
                   currentSenderHop != null && currentRecipientHop != null)
            {
                recipientFutureHops.Add(new DalHopArrival
                {
                    Hop = currentRecipientHop
                });
                currentRecipientHop = currentRecipientHop.PreviousHop;
                senderFutureHops.Add(new DalHopArrival
                {
                    Hop = currentSenderHop
                });
                currentSenderHop = currentSenderHop.PreviousHop;
            }

            // Check if Route was found
            if (currentSenderHop == null || currentRecipientHop == null)
            {
                return null;
            }

            // Build one List
            // Note: senderFutureHops will contain afterwards the full Future Hops List
            senderFutureHops.Add(new DalHopArrival
            {
                Hop = currentSenderHop
            });
            recipientFutureHops.Reverse();
            senderFutureHops.AddRange(recipientFutureHops);
            return senderFutureHops;
        }
    }
}