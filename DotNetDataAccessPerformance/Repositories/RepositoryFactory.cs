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
			if (type == typeof(DrapperNativeRepository))
			{
				return new DrapperNativeRepository();
			}
			if (type == typeof(DataReaderNativeRepository))
			{
				return new DataReaderNativeRepository();
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
			if (type == typeof(NHibernateQueryStrongTypeRepository))
			{
				return new NHibernateQueryStrongTypeRepository();
			}
			if (type == typeof(NHibernateQueryRepository))
			{
				return new NHibernateQueryRepository();
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