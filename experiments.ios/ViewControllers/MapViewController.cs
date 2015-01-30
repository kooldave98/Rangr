using System;

using UIKit;
using CoreLocation;
using CoreGraphics;

using Google.Maps;

namespace experiments.ios
{
    public class MapViewController : BaseViewController
    {
        public MapViewController()
            : base("Map")
        {
           
        }

        MapView mapView;

        public override void LoadView()
        {
            base.LoadView();

            CameraPosition camera = CameraPosition.FromCamera(37.797865, -122.402526, 6);

            mapView = MapView.FromCamera(new CGRect(8, 200, 304, 348), camera);
            mapView.MyLocationEnabled = true;

            var xamMarker = new Marker()
            {
                Title = "Xamarin HQ",
                Snippet = "Where the magic happens.",
                Position = new CLLocationCoordinate2D(37.797865, -122.402526),
                Map = mapView
            };

            mapView.SelectedMarker = xamMarker;

            View = mapView;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
//			mapView.StartRendering ();
        }

        public override void ViewWillDisappear(bool animated)
        {	
//			mapView.StopRendering ();
            base.ViewWillDisappear(animated);
        }
    }
}

