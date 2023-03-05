import axios from 'axios'
import React, { useEffect, useState } from "react";
import { useParams } from 'react-router-dom';

const SportObject = (props) => {
  let { id } = useParams();
  const [sportObject, setSportObject] = useState();
  useEffect(() => {
    axios.get('SportObject/' + id).then((resp) => {

      setSportObject(resp.data);
    });
  }, [setSportObject]);

  if (!sportObject) {
    return "Loading...";
  }

  return (sportObject.name);
};

export default SportObject;