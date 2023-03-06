import axios from 'axios'
import React, { useEffect, useState } from "react";
import { YMaps, Map } from "@pbe/react-yandex-maps";
import resolveReferences from "../resolveReferences"

const Home = (props) => {

  const [sportObjects, setSportObjects] = useState();
  useEffect(() => {
    axios.get('SportObject').then((resp) => {
      setSportObjects(resolveReferences(resp.data));
    });
  }, [setSportObjects]);

  const mapRef = React.createRef(null);

  const initMap = ymaps => {

    if (!(mapRef && mapRef.current)) {
      return;
    }

    var objectManager = new ymaps.ObjectManager({
      clusterize: true,
      zoom: 7
    });

    sportObjects.forEach(sportObject => {
      objectManager.add({
        type: 'Feature',
        id: sportObject.id,
        icon: sportObject.sportObjectType.icon,
        geometry: {
          type: 'Point',
          coordinates: [sportObject.y, sportObject.x]
        },
        properties: {
          hintContent: sportObject.name,
          balloonContentHeader: `${sportObject.name}`,
          balloonContentBody: `<img src="/default.jpg" height="100px">`,
          balloonContentFooter: `
          ${Boolean(sportObject.url) ? '<a href="' + sportObject.url + '">Сайт объекта</a><br>' : ''}
          <b>Тип объекта</b>: ${sportObject.sportObjectType.name}<br>
          ${sportObject.sportTypes?.length > 0 ? '<b>Виды спорта</b>: ' + sportObject.sportTypes.map(sportType => sportType.name).join(', ') + "<br>" : ''}
          ${Boolean(sportObject.workingHoursMondayToFriday) ? '<b>Режим работы Пн.-Пт.:</b> ' + sportObject.workingHoursMondayToFriday + '<br>' : ''}
          ${Boolean(sportObject.workingHoursSaturday) ? '<b>Режим работы Сб.:</b> ' + sportObject.workingHoursSaturday + '<br>' : ''}
          ${Boolean(sportObject.workingHoursSunday) ? '<b>Режим работы Вс.:</b> ' + sportObject.workingHoursSunday + '<br>' : ''}
          <a href="/object/${sportObject.id}">Подробнее</a>
          `
        },
        isActive: sportObject.isActive
      });
    });

    objectManager.objects.each(object => {
      if (object.isActive) {
        objectManager.objects.setObjectOptions(object.id, {
          preset: 'islands#blue' + object.icon + 'Icon'
        });
      } else {
        objectManager.objects.setObjectOptions(object.id, {
          preset: 'islands#red' + object.icon + 'Icon'
        });
      }

    });

    mapRef.current.geoObjects.add(objectManager);
  }

  if (!sportObjects) {
    return "Загрузка...";
  }

  return (<YMaps>
    <Map
      instanceRef={mapRef}
      defaultState={{
        center: [60, 85],
        zoom: 3
      }}
      modules={["ObjectManager", "objectManager.addon.objectsBalloon", "objectManager.addon.objectsHint"]}
      width="100%"
      height="90vh"
      onLoad={ymaps => initMap(ymaps)}
    >
    </Map>
  </YMaps>);
};

export default Home;