﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEC;

namespace GEFExiled.GEFE.API.Interfaces
{
    public interface IGlobalEvent
    {
        /// <summary>
        /// the UNIQUE id of the Global Event
        /// </summary>
        int Id { get; set; }
        /// <summary>
        /// Name used in the logs on the RA
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The description that will be shown to the player when the round start
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// The chance this GE will be choosed at the start of a round
        /// </summary>
        double Weight { get; set; }

        /// <summary>
        /// Is launched at the start of a round
        /// </summary>
        IEnumerator<float> Start();

        void SubscribeEvent();

        void UnsubscribeEvent();
    }
}
