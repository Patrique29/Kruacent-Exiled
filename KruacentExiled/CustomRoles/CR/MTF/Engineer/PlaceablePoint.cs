using Exiled.API.Interfaces;
using KruacentExiled.CustomRoles.CR.MTF.Engineer.Placeables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.CustomRoles.CR.MTF.Engineer
{

    public class PlaceablePoint : IPosition
    {

        private Vector3 _position;
        public Vector3 Position => _position;

        /// <summary>
        /// The <see cref="PlaceableBase"/> currently using this <see cref="PlaceablePoint"/>
        /// </summary>
        public PlaceableBase PlaceableUsingPoint { get; }
        public bool IsEmpty => PlaceableUsingPoint == null;



        public void Place(PlaceableBase placeableBase)
        {

            if(placeableBase == null)
            {
                throw new ArgumentNullException("placeableBase");
            }



        }

        private PlaceablePoint()
        {

        }

        private static HashSet<PlaceablePoint> _allPoints;
        public static IReadOnlyCollection<PlaceablePoint> AllPoints => _allPoints;

        public static void CreateAllPoints()
        {
            _allPoints = new HashSet<PlaceablePoint>();



        }

    }
}
