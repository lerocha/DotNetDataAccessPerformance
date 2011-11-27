using System;

namespace DotNetDataAccessPerformance.Repositories
{
	public class RepositoryFactory
	{
		public static IRepository Create(Type type)
		{
			if (type == typeof(DrapperStoredProcedureRepository))
			{
				return new DrapperStoredProcedureRepository();
			}
			if (type == typeof(DrapperNativeQueryRepository))
			{
				return new DrapperNativeQueryRepository();
			}
			if (type == typeof(DataReaderNativeQueryRepository))
			{
				return new DataReaderNativeQueryRepository();
			}
			if (type == typeof(DataReaderStoredProcedureRepository))
			{
				return new DataReaderStoredProcedureRepository();
			}
			if (type == typeof(EntityFrameworkLinqToEntitiesRepository))
			{
				return new EntityFrameworkLinqToEntitiesRepository();
			}
			if (type == typeof(EntityFrameworkCompiledLinqQueryRepository))
			{
				return new EntityFrameworkCompiledLinqQueryRepository();
			}
			if (type == typeof(EntityFrameworkNativeQueryRepository))
			{
				return new EntityFrameworkNativeQueryRepository();
			}
			if (type == typeof(EntityFrameworkStoredProcedureRepository))
			{
				return new EntityFrameworkStoredProcedureRepository();
			}
			if (type == typeof(NHibernateHqlQueryStrongTypeRepository))
			{
				return new NHibernateHqlQueryStrongTypeRepository();
			}
			if (type == typeof(NHibernateHqlQueryRepository))
			{
				return new NHibernateHqlQueryRepository();
			}
			if (type == typeof(NHibernateNativeQueryRepository))
			{
				return new NHibernateNativeQueryRepository();
			}
			if (type == typeof(NHibernateStoredProcedureRepository))
			{
				return new NHibernateStoredProcedureRepository();
			}
			return null;
		}
	}
}