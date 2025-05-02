import React, { useEffect, useState } from "react";
import L from "leaflet";
import "leaflet/dist/leaflet.css";
import { Container } from "@mui/material";
import "./MapComponent.css";

export function MapComponent({ Info, handler }) {
  const [map, setMap] = useState(null);
  const [IsLocated, setIsLocated] = useState(false);

  useEffect(() => {
    const Map = L.map("map").setView([31.9539, 35.9106], 13);
    L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png").addTo(
      Map
    );

    setMap(Map);

    return () => {
      Map.remove(); // تنظيف الخريطة عند فك التثبيت
    };
  }, []);

  const handleGetLocation = () => {
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(
        (position) => {
          const lat = position.coords.latitude;
          const lng = position.coords.longitude;

          map.eachLayer(function (layer) {
            if (layer instanceof L.Marker) {
              map.removeLayer(layer);
            }
          });

          // إضافة دبوس جديد للموقع الحالي
          L.marker([lat, lng])
            .addTo(map)
            .bindPopup(`موقعك الحالي: ${lat.toFixed(4)}, ${lng.toFixed(4)}`)
            .openPopup();

          // تعيين مركز الخريطة إلى الموقع الحالي
          map.setView([lat, lng], 13);

          handler.setInfo({
            ...Info,
            latitude: lat,
            longitude: lng,
            isChanged: true,
          });
          setIsLocated(true);
        },
        (error) => {
          alert("فشل في الحصول على الموقع: " + error.message);
        }
      );
    } else {
      alert("التحديد الجغرافي غير مدعوم في هذا المتصفح");
    }
  };

  return (
    <div id="Map-div">
      <Container
        id="map"
        sx={{
          height: "300px",
          width: { xs: "100%", lg: "300px" },
        }}
      ></Container>
      <button
        id="location-btn"
        disabled={IsLocated}
        onClick={handleGetLocation}
      >
        {IsLocated ? "تم التحديد" : "حدد موقعي"}
      </button>
    </div>
  );
}

export default MapComponent;
