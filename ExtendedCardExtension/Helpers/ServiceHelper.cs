using System;
using DocsVision.Platform.WebClient.Helpers;
using DocsVision.BackOffice.WebClient.CardKind;
using DocsVision.BackOffice.ObjectModel.Services;

namespace ExtendedCardExtension.Helpers {
    internal class ServiceHelper : DocsVision.BackOffice.WebClient.Helpers.ServiceHelper {
        private ICardKindService cardKindService;
        private IDocumentService documentService;


        public ServiceHelper(IServiceProvider serviceProvider) : base(serviceProvider) {
        }

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

    }
}