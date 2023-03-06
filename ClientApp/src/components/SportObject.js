import axios from 'axios'
import React, { useEffect, useState } from "react";
import { useParams } from 'react-router-dom';
import resolveReferences from "../resolveReferences"
import {
  Card, CardImg, CardText, CardBody,
  CardTitle, CardFooter, CardLink
} from 'reactstrap';

const SportObject = (props) => {
  let { id } = useParams();
  const [sportObject, setSportObject] = useState();
  useEffect(() => {
    axios.get('SportObject/' + id).then((resp) => {
      setSportObject(resolveReferences(resp.data));
    });
  }, [setSportObject]);

  if (!sportObject) {
    return "Загрузка...";
  }

  return (<Card className="text-center">
    <CardImg variant="top" src="/default.jpg" />
    <CardBody>
      <CardTitle tag="h3">{sportObject.name}</CardTitle>
      <CardText>
        <p><b>Тип объекта: </b> {sportObject.sportObjectType.name}</p>
        {sportObject.sportTypes?.length > 0 && <p><b>Виды спорта:</b> {sportObject.sportTypes.map(sportType => sportType.name).join(', ')}</p>}
        {Boolean(sportObject.url) && <p>Сайт: <a href={sportObject.sportObjectDetail.url}>Ссылка</a></p>}
        <p>{sportObject.sportObjectDetail.shortDescription}</p>
        <p>{sportObject.sportObjectDetail.additionalDescription}</p>
      </CardText>
        {Boolean(sportObject.url) && 
      <CardLink href={`${sportObject.url}`}>
      Сайт
    </CardLink>}
    </CardBody>
    <CardFooter className="text-muted">
      {Boolean(sportObject.sportObjectDetail.address) && <p>Адрес: {sportObject.sportObjectDetail.address}</p>}
      {Boolean(sportObject.workingHoursMondayToFriday) && <p>Режим работы Пн.-Пт.: {sportObject.workingHoursMondayToFriday}</p>}
      {Boolean(sportObject.workingHoursSaturday) && <p>Режим работы Сб.: {sportObject.workingHoursSaturday}</p>}
      {Boolean(sportObject.workingHoursSunday) && <p>Режим работы Вс.: {sportObject.workingHoursSunday}</p>}
      {Boolean(sportObject.sportObjectDetail.email) && <p>Email объекта: {sportObject.sportObjectDetail.email}</p>}
      {Boolean(sportObject.sportObjectDetail.phoneNumber) && <p>Номер телефона объекта: {sportObject.sportObjectDetail.phoneNumber}</p>}
      {Boolean(sportObject.sportObjectDetail.oktmo) && <p>ОКТМО: {sportObject.sportObjectDetail.oktmo}</p>}
      {Boolean(sportObject.curator?.name) && <p>Куратор: {sportObject.curator.name}</p>}
      {Boolean(sportObject.curator?.phoneNumber) && <p>Номер телефона куратора: {sportObject.curator.phoneNumber}</p>}
    </CardFooter>
  </Card>);
};

export default SportObject;