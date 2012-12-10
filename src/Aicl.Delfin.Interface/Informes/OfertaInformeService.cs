using System;
using System.Linq;
﻿using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using ServiceStack.ServiceInterface;
using ServiceStack.OrmLite;
using Mono.Linq.Expressions;
using ServiceStack.Common.Web;
using Aicl.Cayita;
using System.Collections.Generic;
using Aicl.Delfin.DataAccess;

namespace Aicl.Delfin.Interface
{
	[RequiresAuthenticateAttribute(ApplyTo.All)]
	public class OfertaInformeService:AppRestService<OfertaInformeRequest>
	{
		public override object OnGet (OfertaInformeRequest request)
		{
			try{

				var visitor = ReadExtensions.CreateExpression<OfertaInforme>();
				var predicate = PredicateBuilder.True<OfertaInforme>();
	
				if(request.Desde!=default(DateTime)){
						predicate= q=>q.FechaEnvio>=request.Desde || q.FechaAceptacion>=request.Desde;
				}
				else
					throw HttpError.Unauthorized("Debe Indicar la fecha de inicio del informe (Desde)");

				if(request.Hasta!=default(DateTime)){
					predicate= predicate.AndAlso(q=>q.FechaEnvio<=request.Hasta || q.FechaAceptacion<=request.Hasta );
				}
				else
					throw HttpError.Unauthorized("Debe Indicar la fecha de terminacion del informe (Hasta)");

				predicate= predicate.AndAlso(q=>q.FechaAnulado==null);

				visitor.Where(predicate);

				return Factory.Execute(proxy=>{
					           
					var ofertas = proxy.Get(visitor); 

					var resumenPorUsuario =(
						from o in ofertas
						group o by o.NombreEnviadoPor into newGroup1
						from newGroup2 in
						(
							from o in newGroup1
							group o by o.Id into gi
							select new {
								FechaEnvio=gi.Max(p=>p.FechaEnvio),
								Valor=gi.Sum(p=>p.Valor*p.Cantidad),
								FechaAceptacion=gi.Max(p=>p.FechaAceptacion),
							}
						)
						group newGroup2 by newGroup1.Key into g
						select new OfertaAgrupada{
							AgrupadaPor=g.Key,
							CantidadEnviada= g.Sum( p=> (p.FechaEnvio>=request.Desde && p.FechaEnvio<=request.Hasta )?1:0),
							ValorEnviado = g.Sum(p=>(p.FechaEnvio>=request.Desde && p.FechaEnvio<=request.Hasta)?p.Valor:0),
							CantidadAceptada= g.Sum( p=> (p.FechaAceptacion>=request.Desde && p.FechaAceptacion<=request.Hasta)?1:0),
							ValorAceptado = g.Sum(p=>(p.FechaAceptacion>=request.Desde && p.FechaAceptacion<=request.Hasta)?p.Valor:0)
						}
					).ToList();

					HtmlGrid<OfertaAgrupada> gridUsuario = 
						BuildGridAgrupadoPor(resumenPorUsuario,
						                     string.Format( "Ofertas por Asesor Comercial<br/> Desde: {0}  Hasta: {1}",
						                                     request.Desde.Format(), request.Hasta.Format()));


					var resumenPorProcedimiento = 
						(from o in ofertas 
						group o by o.NombreProcedimiento into g
						select new  OfertaAgrupada { 
						AgrupadaPor=g.Key, 
						CantidadEnviada= g.Sum( p=> (p.FechaEnvio>=request.Desde && p.FechaEnvio<=request.Hasta )?p.Cantidad:0),
						ValorEnviado = g.Sum(p=>(p.FechaEnvio>=request.Desde && p.FechaEnvio<=request.Hasta)?p.Valor*p.Cantidad:0),
						CantidadAceptada= g.Sum( p=> (p.FechaAceptacion>=request.Desde && p.FechaAceptacion<=request.Hasta)?p.Cantidad:0),
						ValorAceptado = g.Sum(p=>(p.FechaAceptacion>=request.Desde && p.FechaAceptacion<=request.Hasta)?p.Valor*p.Cantidad:0 )
					}).ToList();

					HtmlGrid<OfertaAgrupada> gridProcedimiento = 
						BuildGridAgrupadoPor(resumenPorProcedimiento,
						                     string.Format( "Ofertas por Procedimiento<br/>Desde: {0}  Hasta: {1}",
						                                     request.Desde.Format(), request.Hasta.Format()));

					var resumenPorCliente = (
						from o in ofertas
						group o by o.NombreCliente into newGroup1
						from newGroup2 in
						(
							from o in newGroup1
							group o by o.Id into gi
							select new {
								FechaEnvio=gi.Max(p=>p.FechaEnvio),
								Valor=gi.Sum(p=>p.Valor*p.Cantidad),
								FechaAceptacion=gi.Max(p=>p.FechaAceptacion),
							}
						)
						group newGroup2 by newGroup1.Key into g
						select new OfertaAgrupada{
							AgrupadaPor=g.Key,
							CantidadEnviada= g.Sum( p=> (p.FechaEnvio>=request.Desde && p.FechaEnvio<=request.Hasta )?1:0),
							ValorEnviado = g.Sum(p=>(p.FechaEnvio>=request.Desde && p.FechaEnvio<=request.Hasta)?p.Valor:0),
							CantidadAceptada= g.Sum( p=> (p.FechaAceptacion>=request.Desde && p.FechaAceptacion<=request.Hasta)?1:0),
							ValorAceptado = g.Sum(p=>(p.FechaAceptacion>=request.Desde && p.FechaAceptacion<=request.Hasta)?p.Valor:0)
						}
					).ToList();


					HtmlGrid<OfertaAgrupada> gridCliente = 
						BuildGridAgrupadoPor(resumenPorCliente,
						                     string.Format( "Ofertas por Cliente<br/>Desde: {0}  Hasta: {1}",
						                                     request.Desde.Format(), request.Hasta.Format()));

					//----------------------------------
					var empresa=proxy.GetEmpresa();

					var pedidos=(
						from p in ofertas
						group p by p.Id into g
						select new  OfertaInforme{
						Valor= g.Sum(f=>f.Valor*f.Cantidad),
						NombreCliente= g.Max(f=>f.NombreCliente),
						Consecutivo =  g.Max(f=>f.Consecutivo),
						FechaAceptacion =  g.Max(f=>f.FechaAceptacion),
						NombreEnviadoPor= g.Max(f=>f.NombreEnviadoPor),

					}).ToList();



					HtmlGrid<OfertaInforme> gridOfertas = new HtmlGrid<OfertaInforme>();
					gridOfertas.DataSource= pedidos.OrderByDescending(f=>f.Valor).ToList();
					gridOfertas.Css = new CayitaGridGrey();
					gridOfertas.Title=  string.Format( "Relacion de Ofertas Enviadas<br/>Desde: {0}  Hasta: {1}",
						                                     request.Desde.Format(), request.Hasta.Format());

					var gc = gridOfertas.CreateGridColumn();
					gc.HeaderText="Consecutivo";
					gc.CellRenderFunc=(row, index)=>row.Consecutivo.Format();
					gc.FooterRenderFunc=()=> pedidos.Count.Format();
					gridOfertas.AddGridColum(gc);


					gc = gridOfertas.CreateGridColumn();
					gc.HeaderText="Cliente";
					gc.CellRenderFunc=(row, index)=>row.NombreCliente;
					gridOfertas.AddGridColum(gc);
								
					gc = gridOfertas.CreateGridColumn();
					gc.HeaderText="Valor";
					gc.CellRenderFunc=(row, index)=>row.Valor.Format();
					gc.FooterRenderFunc= ()=> pedidos.Sum(f=>f.Valor).Format();
					gridOfertas.AddGridColum(gc);

					gc = gridOfertas.CreateGridColumn();
					gc.HeaderText="Aceptada?";
					gc.CellRenderFunc=(row, index)=> {
						if (row.FechaAceptacion.HasValue) {
							var img = 	new HtmlImage{Url=empresa.ApplicationHost+"/resources/icons/fam/accept.png" };
							var p = new HtmlParagragh{Style=new HtmlStyle{TextAlign="center"} };
							p.AddHtmlTag(img);
							return p.ToString();
						}

						return "";
					};
					gc.FooterRenderFunc=()=> pedidos.Count(f=>f.FechaAceptacion.HasValue).Format();
					gridOfertas.AddGridColum(gc);


					gc=gridOfertas.CreateGridColumn();
					gc.HeaderText="Asesor";
					gc.CellRenderFunc=(row, index)=>row.NombreEnviadoPor;
					gridOfertas.AddGridColum(gc);


					return new HtmlResponse{
						Html= gridUsuario.ToString()+
							"<br/>"+ gridCliente.ToString()+
							"<br/>"+ gridOfertas.ToString()+
							"<br/>"+ gridProcedimiento.ToString()
                	};

				});
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<OfertaInforme>>(e,"GetOfertaInformeError");
			}
		}

		public override object OnPost (OfertaInformeRequest request)
		{
			return OnGet (request);
		}

		HtmlGrid<OfertaAgrupada> BuildGridAgrupadoPor(List<OfertaAgrupada> dataSource, string title){

			HtmlGrid<OfertaAgrupada> grid = new HtmlGrid<OfertaAgrupada>() ;
			grid.DataSource=dataSource;
			grid.Css = new CayitaGridGrey();
			grid.Title=title;

			var gc = grid.CreateGridColumn();
			gc.HeaderText="Nombre";
			gc.CellRenderFunc=(f,rowIndex)=>f.AgrupadaPor;
			grid.AddGridColum(gc);

			gc = grid.CreateGridColumn();
			gc.HeaderText="Enviadas #";
			gc.CellRenderFunc=(f,rowIndex)=>f.CantidadEnviada.Format();
			gc.FooterRenderFunc = ()=>  dataSource.Sum(f=>f.CantidadEnviada).Format();
			grid.AddGridColum(gc);

			gc = grid.CreateGridColumn();
			gc.HeaderText="Enviadas $";
			gc.CellRenderFunc=(f,rowIndex)=>f.ValorEnviado.Format();
			gc.FooterRenderFunc = ()=>  dataSource.Sum(f=>f.ValorEnviado).Format();
			grid.AddGridColum(gc);

			gc = grid.CreateGridColumn();
			gc.HeaderText="Aceptadas #";
			gc.CellRenderFunc=(f,rowIndex)=>f.CantidadAceptada.Format();
			gc.FooterRenderFunc = ()=>  dataSource.Sum(f=>f.CantidadAceptada).Format();
			grid.AddGridColum(gc);

			gc = grid.CreateGridColumn();
			gc.HeaderText="Aceptadas $";
			gc.CellRenderFunc=(f,rowIndex)=>f.ValorAceptado.Format();
			gc.FooterRenderFunc = ()=>  dataSource.Sum(f=>f.ValorAceptado).Format();
			grid.AddGridColum(gc);

			return grid;


		}

	}
}
