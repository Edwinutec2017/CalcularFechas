import React, { Component, Fragment } from 'react';
import './Home.css';


export class Home extends Component {
  static displayName = Home.name;

    constructor(props)
    {
        super(props)
        this.state = { fechaferiado: '', datafecha: [], mensage:"Seleccione una fecha del calendario",count:0,anio:'' };
        this.AddDate = this.AddDate.bind(this);
        this.capturadatos = this.capturadatos.bind(this);
        this.DeleteDate = this.DeleteDate.bind(this);
        this.TableDate = this.TableDate.bind(this);
        this.EnvioDataController = this.EnvioDataController.bind(this);
        this.capturaanio = this.capturaanio.bind(this);
        this.UploadFile = this.UploadFile.bind(this);
    }

    capturadatos(event) { this.setState({ fechaferiado: event.target.value }); }
    capturaanio(event) { this.setState({ anio: event.target.value }); }

    AddDate()
    {

        if (this.state.fechaferiado === '')
            this.state.mensage = "Seleccione una fecha del calendario ";
        else
        {
            var fecha = this.state.fechaferiado;
            this.state.datafecha.push(fecha);
            this.state.mensage = "Fecha agregada :" + this.state.datafecha[this.state.count];
            this.setState({
                count : this.state.count + 1
            });
          
            this.refs.dias.value = '';
        }
     
    }

    EnvioDataController() {
        if (this.state.datafecha.length > 0 && this.state.anio !== null && this.state.anio.length ===4) {
            var fechas = { Fechas: this.state.datafecha, Anio: this.state.anio };
        
            fetch("api/CalculoDias/CalculoFechas", {
                "method": "POST",
                "headers": {

                    "content-type": "application/json",
                    "accept": "application/json"
                },
                "body": JSON.stringify(fechas)
            })
                .then(response => response.json())
                .then(response => {
                    this.UploadFile(response);
                })
                .catch(err => {
                    console.log(err);
                });

        } else
        {
            if (this.state.anio < 4)
                console.log("El a&ntilde;o no tiene la longitud corecta");
        }
    };

    DeleteDate(fechaDelete)
    {
        this.setState({
            mensage: "Eliminado ",
            count: this.state.count - 1,
            datafecha: this.state.datafecha.filter(function (fecha) { return fecha !== fechaDelete })
        });
    }

    UploadFile(file)
    {
        if (file.length > 0)
        {
            this.setState({ datafecha: [], mensage: "Se crearon los archivos exitosamente", count:0 });
            this.refs.anio.value = '';

            for (var count = 0; count <  file.length; count++)
            {
                const link = `data:application/sql;base64,${file[count].base64}`;
                const dowdl = document.createElement("a");
                const fileName = file[count].name;
                dowdl.href = link;
                dowdl.download = fileName;
                dowdl.click();
                dowdl.remove();
            }
        }
    }

    render() {
        let contents;
        if (this.state.datafecha.length > 0)
        {
            contents = this.state.loading
                ? <p><em>Loading...</em></p>
                : this.TableDate(this.state.datafecha);
        }

        return (
            <div class="container">
                <div class="row">
                    <div class="col-md-12">
                        <h3>GENERACION DE CALENDARIO DEL SEPP NOTIFICACIONES!!</h3>
                    </div>
                    <div class="col-md-7">
                        A&ntilde;o a calcular: <input type="number" placeholder="YYYY" ref="anio" onChange={this.capturaanio} />
                        Dias feriados : <input type="date" ref="dias" onChange={this.capturadatos} />
                    </div>

                    <div class="col-md-5">
                        <button className="btn btn-primary" onClick={this.AddDate}>Agregar.</button>
                        <button className="btn btn-warning" onClick={this.EnvioDataController}>Calcular fecha</button>
                    </div>
                    <div class="col-md-12">
                        <p aria-live="polite"><strong>{this.state.mensage}</strong></p>
                    </div>
                    <div class="col-md-12">
                        {contents}
                    </div>

                </div>
            </div>
    );
    }

     TableDate(fechas) {
 
        return (

            <table class="table table-striped"  id="fechas">
                    <thead>
                        <tr>
                            <th>Fechas</th>
                            <th>Accion</th>

                        </tr>
                    </thead>
                    <tbody>
                        {fechas.map(fechas =>
                            <tr key={fechas}>
                                <td>{fechas}</td>
                                <td><button className="btn btn-primary" onClick={() => this.DeleteDate(fechas)} >Eliminar</button></td>
                            </tr>
                        )}
                    </tbody>
                </table>
          
            );
    }

}
