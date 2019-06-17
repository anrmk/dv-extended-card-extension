using System;
using DocsVision.ApprovalDesigner.ObjectModel.Mapping;
using DocsVision.Platform.Data.Metadata;
using DocsVision.Platform.ObjectManager;
using DocsVision.Platform.ObjectModel;
using DocsVision.Platform.ObjectModel.Mapping;
using DocsVision.Platform.ObjectModel.Persistence;
using DocsVision.BackOffice.ObjectModel.Services;
using DocsVision.BackOffice.ObjectModel;
using DocsVision.BackOffice.ObjectModel.Mapping;
using DocsVision.Platform.SystemCards.ObjectModel.Mapping;
using DocsVision.Platform.SystemCards.ObjectModel.Services;


namespace ExtendedCardExtension {
    /// <summary>
    /// Represents session context
    /// </summary>
    public sealed class SessionContext : IServiceProvider {
        private ObjectContext objectContext;
        private UserSession session;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionContext"/> class
        /// </summary>
        public SessionContext() {

        }

        #region Properties

        /// <summary>
        /// Gets user session
        /// </summary>
        public UserSession Session {
            get {
                return session;
            }
        }

        /// <summary>
        /// Gets object context
        /// </summary>
        public ObjectContext ObjectContext {
            get {
                if (objectContext == null)
                    CreateObjectContext();
                return objectContext;
            }
        }

        public void DisposeObjectContext() {
            if (objectContext != null) {
                objectContext.Dispose();
                objectContext = null;
            }
        }


        // TODO: Replace ConnectionString by BaseAlias
        /// <summary>
        /// Gets tenant identifier
        /// </summary>
        public string BaseAlias {
            get {
                return this.Session.Connection.ConnectionString;
            }
        }

        #endregion


        /// <summary>
        /// Initializes session context
        /// </summary>
        public void Initialize(UserSession session) {
            this.session = session;
            objectContext = null;
        }

        /// <summary>
        /// Closes session context
        /// </summary>
        public void Close() {
            if (objectContext != null)
                objectContext.Dispose();

            objectContext = null;
            session = null;
        }

        private void CreateObjectContext() {
            {
                try {
                    objectContext = new ObjectContext(this);

                    IObjectMapperFactoryRegistry mapperFactoryRegistry = objectContext.GetService<IObjectMapperFactoryRegistry>();

                    mapperFactoryRegistry.RegisterFactory(typeof(SystemCardsMapperFactory));
                    mapperFactoryRegistry.RegisterFactory(typeof(BackOfficeMapperFactory));
                    mapperFactoryRegistry.RegisterFactory(typeof(ApprovalDesignerMapperFactory));

                    IServiceFactoryRegistry serviceFactoryRegistry = objectContext.GetService<IServiceFactoryRegistry>();

                    serviceFactoryRegistry.RegisterFactory(typeof(SystemCardsServiceFactory));
                    serviceFactoryRegistry.RegisterFactory(typeof(BackOfficeServiceFactory));
                    serviceFactoryRegistry.RegisterFactory(typeof(ApprovalDesignerServiceFactory));

                    objectContext.AddService(DocsVisionObjectFactory.CreatePersistentStore(Session));

                    IMetadataProvider metadataProvider = DocsVisionObjectFactory.CreateMetadataProvider(new SessionProvider(Session));
                    objectContext.AddService(metadataProvider);
                    objectContext.AddService(DocsVisionObjectFactory.CreateMetadataManager(metadataProvider, Session));

                    objectContext.GetService<IAccessCheckingService>().EditMode = true;
                } catch (Exception) {
                    if (objectContext != null) {
                        objectContext.Dispose();
                        objectContext = null;
                    }

                    // TODO: Log exception
                    throw;
                }
            }
        }

        /// <summary>
        /// Get service by type
        /// </summary>
        /// <param name="serviceType">service type</param>
        /// <returns>specified service instance</returns>
		public object GetService(Type serviceType) {
            return serviceType == typeof(UserSession) ? Session : null;
        }
    }
}
