
using Aicl.Delfin.Model.Types;
using Aicl.Delfin.Model.Operations;
using System;
using ServiceStack.OrmLite;
using System.Linq;
using Aicl.Cayita;
using Aicl.Delfin.DataAccess;

namespace Aicl.Delfin.Interface
{
	public class ClienteProcedimientoService:AppRestService<ClienteProcedimientoRequest>
	{
		public override object OnGet (ClienteProcedimientoRequest request)
		{

			try{

				Empresa empresa= new Empresa();

				var visitor = ReadExtensions.CreateExpression<OfertaInforme>();

				visitor.Where(q=>q.IdCliente==request.Id);

				var procs= Factory.Execute(proxy=>{
					empresa = proxy.GetEmpresa();
					return proxy.Get(visitor);
				});
	
				HtmlGrid<OfertaInforme> grid = new HtmlGrid<OfertaInforme>();
				grid.DataSource= procs.OrderByDescending(f=>f.Consecutivo).ToList();
				grid.Css = new CayitaGridGrey();
				grid.Title= "Procedimientos Ofertados" ;
				var gc = grid.CreateGridColumn();
				gc.HeaderText="Consecutivo";
				gc.CellRenderFunc=(row, index)=>row.Consecutivo.Format();
				gc.FooterRenderFunc=()=> procs.Count.Format();
				grid.AddGridColum(gc);
									
				gc = grid.CreateGridColumn();
				gc.HeaderText="Procedimiento";
				gc.CellRenderFunc=(row, index)=>row.NombreProcedimiento;
				grid.AddGridColum(gc);

				gc = grid.CreateGridColumn();
				gc.HeaderText="Ctdad";
				gc.CellRenderFunc=(row, index)=>row.Cantidad.Format();
				grid.AddGridColum(gc);

				gc = grid.CreateGridColumn();
				gc.HeaderText="V/Unit $";
				gc.CellRenderFunc=(row, index)=>row.Valor.Format();
				grid.AddGridColum(gc);

				gc = grid.CreateGridColumn();
				gc.HeaderText="Total $";
				gc.CellRenderFunc=(row, index)=>(row.Valor*row.Cantidad).Format();
				grid.AddGridColum(gc);

				gc = grid.CreateGridColumn();
				gc.HeaderText="Enviada?";
				gc.CellRenderFunc=(row, index)=> {
					if (row.FechaEnvio.HasValue) {
						var img = 	new HtmlImage{Url=empresa.ApplicationHost+"/resources/icons/fam/accept.png" };
						var p = new HtmlParagragh{Style=new HtmlStyle{TextAlign="center"} };
						p.AddHtmlTag(img);
						return p.ToString();
					}

					return "";
				};
				grid.AddGridColum(gc);

				gc = grid.CreateGridColumn();
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
				grid.AddGridColum(gc);

				return new HtmlResponse{
					Html= grid.ToString()
				};
			}
			catch(Exception e){
				return HttpResponse.ErrorResult<Response<ClienteProcedimientoRequest>>(e,"GetClienteProcedimientoError");
			}

		}

		public override object OnPost (ClienteProcedimientoRequest request)
		{
			return OnGet (request);
		}

	}
}

