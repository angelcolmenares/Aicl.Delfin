using System;
using ServiceStack.DataAnnotations;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.ServiceHost;

namespace Aicl.Delfin.Model.Types
{
	[RestService("/OfertaInforme/read","get")]
	public class OfertaInformeRequest{

		public OfertaInformeRequest(){}
		public DateTime Desde{get;set;}
		public DateTime Hasta{get;set;}

	}

	[RestService("/ClienteProcedimiento/{Id}","get,post")]
	public class ClienteProcedimientoRequest{

		public ClienteProcedimientoRequest(){}
		public int Id{get;set;}

	}



	public class OfertaAgrupada{

		public OfertaAgrupada(){}
		public string AgrupadaPor {get;set;}
		public int CantidadEnviada {get;set;}
		public decimal ValorEnviado {get;set;}
		public int CantidadAceptada {get;set;}
		public decimal ValorAceptado {get;set;}
	}


	[Alias("Pedido")]
	[JoinTo(typeof(Contacto),"IdContacto","Id",Order=0)]
	[JoinTo(typeof(Contacto),typeof(Cliente),"IdCliente","Id", Order=1)]
	[JoinTo(typeof(UserAuth),"IdEnviadoPor","Id", ChildAlias="SendBy", Order=2)]
	[JoinTo(typeof(PedidoItem),"Id","IdPedido",Order=3)]
	[JoinTo(typeof(PedidoItem), typeof(Procedimiento),"IdProcedimiento","Id", Order=4) ]
	public class OfertaInforme
	{

		public OfertaInforme ()
		{
		}

		public int Id {get;set;}
		public int IdCreadoPor {get;set;}
		public int IdContacto {get;set;}
		public int IdEnviadoPor {get;set;}
		public int Consecutivo {get;set;}

		public DateTime? FechaEnvio{get;set;}
		public DateTime? FechaAceptacion {get;set;}
		public DateTime? FechaAnulado {get;set;}


		[BelongsTo(typeof(PedidoItem))]
		public int IdPedido {get; set;}
		[BelongsTo(typeof(PedidoItem),"ValorUnitario")]
		public decimal Valor {get; set;}
		[BelongsTo(typeof(PedidoItem))]
		public decimal PorcentajeIva {get; set;}
		[BelongsTo(typeof(PedidoItem))]
		public decimal Descuento {get; set;}
		[BelongsTo(typeof(PedidoItem))]
		public int Cantidad {get;set;}

		[BelongsTo(typeof(UserAuth),"DisplayName", ParentAlias="SendBy")]
		public string NombreEnviadoPor {get;set;}

		[BelongsTo(typeof(Cliente),"Nombre")]
        public string NombreCliente {get;set;}

		[BelongsTo(typeof(Cliente),"Id")]
        public int IdCliente {get;set;}

		[BelongsTo(typeof(PedidoItem))]
		public int IdServicio {get;set;}
		[BelongsTo(typeof(PedidoItem))]
		public int IdProcedimiento {get;set;}

		[BelongsTo(typeof(Procedimiento),"Nombre")]
		public string NombreProcedimiento {get; set;}

	}
}
/*

SELECT 
IdCreadoPor, 
IdServicio,
IdProcedimiento,
ValorUnitario, 
Cantidad,
PorcentajeIva,
Descuento,
FechaEnvio,
FechaAceptacion,
FechaAnulado 
FROM  Pedido
join PedidoItem
on PedidoItem.IdPedido=Pedido.Id
-- where Pedido.IdCreadoPor=4
where 
(
(FechaEnvio>='2012.10.01' and FechaEnvio<='2012.10.31')
or 
(FechaAceptacion>='2012.10.01' and FechaAceptacion<='2012.10.31')
)
and 
FechaAnulado is  null 


*/
