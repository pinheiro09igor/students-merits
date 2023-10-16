import "./ListarVantagens.css";

import { CircularProgress } from "@mui/material";

import CardVantagem from "./components/cards/CardVantagem";
import Searchbar from "../../components/searchbars/SearchBar";
import { Slider } from "@mui/material";

import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

const url = "http://localhost:7077";

const ListarVantagens = () => {
  const [vantagens, setVantagens] = useState([]),
    [isLoading, setIsLoading] = useState(true),
    [searchValue, setSearchValue] = useState(""),
    [value, setValue] = useState([0, 100]),
    [min, setMin] = useState(0),
    [max, setMax] = useState(0),
    [minDistance] = useState(20),
    step = 20;

  const navigate = useNavigate();

  useEffect(() => {
    fetch(`${url}/api/vantagens`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
    })
      .then((resp) => {
        return resp.json();
      })
      .then((data) => {
        setVantagens(data);
        const precos = data.map((data) => data.preco);

        const min = Math.min(...precos);
        const max = Math.max(...precos);

        setMin(min);
        setMax(max);

        setValue([min, max]);
      })
      .then(() => {
        setTimeout(() => {
          setIsLoading(false);
        }, 600);
      });
  }, []);

  const handleChange = (e, newValue, activeThumb) => {
    if (!Array.isArray(newValue)) {
      return;
    }

    if (activeThumb === 0) {
      setValue([Math.min(newValue[0], value[1] - minDistance), value[1]]);
    } else {
      setValue([value[0], Math.max(newValue[1], value[0] + minDistance)]);
    }
  };

  const handleOnClick = (_id) => {
    navigate("/trocarVantagem/" + _id);
  };

  const filter = (arr, searchValue, value) => {
    if (searchValue)
      arr = arr.filter((elem) =>
        elem.nome.toLowerCase().includes(searchValue.toLowerCase())
      );

    return arr.filter(
      (elem) => elem.preco >= value[0] && elem.preco <= value[1]
    );
  };

  const handleChangeSearchValue = (newValue) => {
    setSearchValue(newValue);
  };

  return (
    <div className="body-listar-vantagem flex flex-column align-center">
      <section className="title-section flex space-between column-gap-3rem align-center">
        <div className="section-1 flex flex-column row-gap-1rem">
          <h1 className="title">
            Troque por vantagens<span>.</span>
          </h1>
          <p className="description-listar-vantagem">
            Troque suas moedas aculumadas por vantagens como desconto em
            restaurantes da universidade, desconto de mensalidade, ou compra de
            materiais específicos.
          </p>

          <div>
            <Searchbar
              placeholder="Digite uma vantagem..."
              onChange={handleChangeSearchValue}
            />
          </div>
        </div>
        <img
          src="/sacola.png"
          alt="Troque moedas por vantagens"
          className="bag-img"
        />
      </section>

      <section className="content flex flex-column row-gap-3rem">
        {isLoading ? (
          <CircularProgress />
        ) : (
          <>
            <div className="filter-cards flex flex-column row-gap-1rem">
              <span>Filtrar por moedas</span>
              <Slider
                value={value}
                onChange={handleChange}
                valueLabelDisplay="on"
                step={step}
                min={min}
                max={max}
              />
            </div>

            <section className="cards flex flex-wrap column-gap-3rem row-gap-3rem space-between">
              {filter(vantagens, searchValue, value).map((vantagem) => (
                <CardVantagem
                  key={Math.random()}
                  content={vantagem}
                  onClick={handleOnClick}
                />
              ))}
            </section>
          </>
        )}
      </section>
    </div>
  );
};

export default ListarVantagens;
