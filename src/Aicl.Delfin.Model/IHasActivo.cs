using ServiceStack.DesignPatterns.Model;

namespace Aicl.Delfin.Model.Types
{
	public interface IHasActivo:IHasId<int>
	{
		bool Activo {get;set;}
	}
}

