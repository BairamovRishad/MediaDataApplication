using System.Data;
using System.Data.Common;
using MediaDataApplication.WcfService.DAL.Mappers;
using MediaDataApplication.WcfService.DAL.Repository;

namespace MediaDataApplication.WcfService.DAL.DAO {

    public abstract class BaseDAO {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        protected BaseDAO() {
            try {
                App.InitDataDirectory();
                this.unitOfWork = new EfUnitOfWork(new MediaDataDbContext());
                this.mapper = new CommonMapper();
            }
            catch {
                throw;
            }
        }

        // For tests. I thought that it would by unnecessary to use any IoC only for UnitOfWork
        protected BaseDAO(IUnitOfWork unitOfWork) {
            this.unitOfWork = unitOfWork;
            this.mapper = new CommonMapper();
        }

        protected IUnitOfWork UnitOfWork {
            get {
                return this.unitOfWork;
            }
        }

        protected IMapper Mapper {
            get {
                return this.mapper;
            }
        }
    }

}