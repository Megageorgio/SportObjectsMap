import axios from 'axios'
import React, { useEffect, useState } from "react";
import resolveReferences from "../resolveReferences"
import { LineChart, Line, CartesianGrid, XAxis, YAxis, ResponsiveContainer, Tooltip, Legend, BarChart, Bar } from 'recharts';
import { Container, Row, Col } from 'react-grid-system';

const Stats = (props) => {
  const [stats, setStats] = useState();
  useEffect(() => {
    axios.get('Stats/').then((resp) => {

      setStats(resolveReferences(resp.data))
    });
  }, [setStats]);

  if (!stats) {
    return "Загрузка...";
  }

  return (
    <Container>
      <Row style={{ height: '40vh' }}>
        <Col>
          <ResponsiveContainer>
            <LineChart
              layout="horizontal"
              data={stats}
            >
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="value" type="category" />
              <YAxis type="number" />
              <Tooltip />
              <Legend />
              <Line name="Построено спортивных объектов" data={stats.objectBuildingStat} dataKey="count" stroke="blue" />
            </LineChart>
          </ResponsiveContainer>
        </Col>
      
      </Row>
      <Row style={{ height: '40vh' }}>
      <Col>
        <ResponsiveContainer>
            <LineChart
              layout="horizontal"
              data={stats}
            >
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="value" type="category" />
              <YAxis type="number" />
              <Tooltip />
              <Legend />
              <Line name="Реконструировано спортивных объектов" data={stats.objectReconstructingStat} dataKey="count" stroke="red" />
            </LineChart>
          </ResponsiveContainer>
        </Col>
      </Row>
      <Row style={{ height: '40vh' }}>
        <Col>
        <ResponsiveContainer>
            <BarChart
              layout="horizontal"
              data={stats}
            >
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="value" type="category" tick={{ fontSize: "9px" }} />
              <YAxis type="number" />
              <Tooltip />
              <Legend />
              <Bar name="Типы спортивных объектов" data={stats.objectTypeStat} dataKey="count" fill="orange" />
            </BarChart>
          </ResponsiveContainer>
        </Col>
      </Row>
      <Row style={{ height: '40vh' }}>
      <Col>
        <ResponsiveContainer>
            <BarChart
              layout="horizontal"
              data={stats}
            >
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="value" type="category" tick={{ fontSize: "9px" }} />
              <YAxis type="number" />
              <Tooltip />
              <Legend />
              <Bar name="Виды спорта в спортивных объектах" data={stats.sportTypeStat} dataKey="count" fill="green" />
            </BarChart>
          </ResponsiveContainer>
        </Col>
    </Row>
    </Container>
  );
};

export default Stats;