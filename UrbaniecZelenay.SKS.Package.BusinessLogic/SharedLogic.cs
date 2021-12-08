using System;
using System.Collections.Generic;
using System.Linq;
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
            var recipientTime = new List<int>();
            var senderTime = new List<int> { 0 };
            // Build Future Hops hierarchy
            while (currentRecipientHop?.Code != currentSenderHop?.Code &&
                   currentSenderHop != null && currentRecipientHop != null)
            {
                recipientFutureHops.Add(new HopArrival
                {
                    Hop = currentRecipientHop,
                });
                if (currentRecipientHop.PreviousHop != null)
                {
                    // B <- A consist of A-B travel time and B delay
                    // (A is currentRecipientHop)
                    recipientTime.Add(
                        currentRecipientHop.PreviousHop.Hop.ProcessingDelayMins!.Value +
                        currentRecipientHop.PreviousHop.TraveltimeMins
                    );
                }

                currentRecipientHop = currentRecipientHop.PreviousHop?.Hop;

                senderFutureHops.Add(new HopArrival
                {
                    Hop = currentSenderHop,
                });
                if (currentSenderHop.PreviousHop != null)
                {
                    // A -> B consists of A delay and A-B travel time
                    // (A is currentSenderHop)
                    senderTime.Add(
                        currentSenderHop.ProcessingDelayMins!.Value +
                        currentSenderHop.PreviousHop.TraveltimeMins
                    );
                }

                currentSenderHop = currentSenderHop.PreviousHop?.Hop;
            }

            // Check if Route was found
            if (currentSenderHop == null || currentRecipientHop == null)
            {
                return null;
            }

            // Build one List for Hop Arrivals / Time
            senderFutureHops.Add(new HopArrival
            {
                Hop = currentSenderHop
            });
            recipientTime.Reverse();
            senderTime.AddRange(recipientTime);
            // Note: senderFutureHops will contain afterwards the full Future Hops List
            recipientFutureHops.Reverse();
            senderFutureHops.AddRange(recipientFutureHops);
            
            // Calculate time
            // Note: We are very optimistic, that the pickup will be tomorrow
            var time = DateTime.Now.AddDays(1);
            // Use Zip for convenience
            // See: https://stackoverflow.com/a/1955780/12347616
            var hopsAndTime = senderFutureHops.Zip(senderTime, (arrival, i) => new
            {
                HopArrival = arrival, Time = i
            });
            foreach (var ht in hopsAndTime)
            {
                time = time.AddMinutes(ht.Time);
                ht.HopArrival.DateTime = time;
            }

            return senderFutureHops;
        }
    }
}