using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.CustomRoles.CR.MTF.Engineer.Placeables
{
    public abstract class PlaceableBase 
    {
        public PlaceablePoint PointPlacedOn { get; private set; } = null;

        public Vector3 Position => PointPlacedOn?.Position ?? Vector3.zero;

        public float RotationY { get; private set; }






        public bool TryPlace(PlaceablePoint pointToPlaceOn,float rotationY)
        {

            if(pointToPlaceOn == null)
            {
                throw new ArgumentNullException("pointToPlaceOn");
            }

            if (pointToPlaceOn.IsEmpty)
            {
                return false;
            }

            pointToPlaceOn.Place(this);

            PointPlacedOn = pointToPlaceOn;

            RotationY = rotationY;
            return true;
        }




    }
}
