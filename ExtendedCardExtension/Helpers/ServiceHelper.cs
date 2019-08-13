using System;
using DocsVision.Platform.WebClient.Helpers;
using DocsVision.BackOffice.WebClient.CardKind;
using DocsVision.BackOffice.ObjectModel.Services;

namespace ExtendedCardExtension.Helpers {
    internal class ServiceHelper : DocsVision.BackOffice.WebClient.Helpers.ServiceHelper {
        private ICardKindService cardKindService;
        private IDocumentService documentService;
        private ITaskService taskService;
        private IStateService stateService;
        private ITaskListService taskListService;
        private IStaffService staffService;

        public ServiceHelper(IServiceProvider serviceProvider) : base(serviceProvider) { }

        public ICardKindService CardKindService {
            get {
                return cardKindService ?? (cardKindService = ServiceUtil.GetService<ICardKindService>(serviceProvider));
            }
        }

        public IDocumentService DocumentService {
            get {
                return documentService ?? (documentService = ServiceUtil.GetService<IDocumentService>(serviceProvider));
            }
        }

        public ITaskService TaskService {
            get {
                return taskService ?? (taskService = ServiceUtil.GetService<ITaskService>(serviceProvider));
            }
        }

        public IStateService StateService {
            get {
                return stateService ?? (stateService = ServiceUtil.GetService<IStateService>(serviceProvider));
            }
        }

        public ITaskListService TaskListService {
            get {
                return taskListService ?? (taskListService = ServiceUtil.GetService<ITaskListService>(serviceProvider));
            }
        }

        public IStaffService StaffService {
            get {
                return staffService ?? (staffService = ServiceUtil.GetService<IStaffService>(serviceProvider));
            }
        }
    }
}