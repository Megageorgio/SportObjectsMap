import axios from 'axios'
import React, { useEffect, useState } from "react";

const Stats = (props) => {
  const [stats, setStats] = useState();
  useEffect(() => {
    axios.get('Stats/').then((resp) => {

      setStats(resp.data);
    });
  }, [setStats]);

  if (!stats) {
    return "Loading...";
  }

  return ("test");
};

export default Stats;