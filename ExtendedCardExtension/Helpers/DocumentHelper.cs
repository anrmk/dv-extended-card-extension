using DocsVision.BackOffice.CardLib.CardDefs;
using DocsVision.BackOffice.ObjectModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ExtendedCardExtension.Helpers {
    public class DocumentHelper {
        private readonly Document document;
        private readonly SessionContext context;

        public DocumentHelper(Document doc, SessionContext context) {
            this.context = context;
            if (doc != null)
                document = doc;
        }

        public Document GetDocument => document;

        /// <summary>
        /// Секция "Основная информация"
        /// </summary>
        public DocumentMainInfo GetMainInfo => GetDocument?.MainInfo;

        /// <summary>
        /// Секция "Системная информация"
        /// </summary>
        public BaseCardSystemInfo SystemInfo => GetDocument?.SystemInfo;

        /// <summary>
        /// Наименование (предмет договора)
        /// </summary>
        public string Name => GetMainInfo.Name;

        public string ItemName => GetMainInfo.Item?.Name;

        /// <summary>
        /// Состояние документа
        /// </summary>
        public StatesState State => SystemInfo.State;

        /// <summary>
        /// Наименование объекта (Описание)
        /// </summary>
        public string Description {
            get {
                var description = "";
                if (GetMainInfo[CardDocument.MainInfo.ItemID] != null) {
                    BaseUniversalItem item = (BaseUniversalItem)GetMainInfo[CardDocument.MainInfo.ItemID];
                    if (item != null)
                        description = item.Name;
                }

                var baseUniversal = document.GetSection(new Guid("2A157825-4B9C-44B1-B0A5-10364A502944"));
                if (baseUniversal != null) {
                    foreach (BaseCardSectionRow tableRow in baseUniversal) {
                        Guid itemId = Guid.Empty;
                        if (Guid.TryParse(tableRow["Equipment"] + "", out itemId)) {
                            BaseUniversalItem item = this.context.ObjectContext.GetObject<BaseUniversalItem>(itemId);
                            if (item != null)
                                description += $", {item.Name}";
                        }
                    }
                }

                //определение пятна
                var controlSection = GetDocument.GetSection(CardDocument.Control.ID);
                if (controlSection.Count > 0) {
                    BaseCardSectionRow controlRow = (BaseCardSectionRow)controlSection[0];
                    if (!string.IsNullOrEmpty(controlRow["String2"] + ""))
                        description += $"Пятно: {controlRow["String2"]}";
                }

                return description;
            }
        }

        /// <summary>
        /// Наименование Документа, номер и дата
        /// </summary>
        public string DocumentName => string.Format($"{SystemInfo?.CardKind.Name} {0}", GetDocument?.Numbers.Select(x => $"№ {x.Number} от { GetMainInfo.DeliveryDate.ToShortDateString() }").FirstOrDefault());

        public string RegistrarName => GetMainInfo.Registrar?.DisplayName;
        public string AuthorName => GetMainInfo.Author?.DisplayName;

        public string CreatedDate => GetDocument.CreateDate.ToString("dd.MM.yyyy HH:mm:ss");

        /// <summary>
        /// Заказчик
        /// </summary>
        public string ResponseStaffName {
            get {
                Guid organizationId = Guid.Empty;
                if (Guid.TryParse(GetMainInfo[CardDocument.MainInfo.ResponsDepartment] + "", out organizationId)) {
                    StaffUnit unit = context.ObjectContext.GetObject<StaffUnit>(organizationId);
                    return unit.Name;
                }
                return "";
            }
        }

        public string[] PartnersCompany {
            get {
                List<PartnersCompany> partners = new List<PartnersCompany>();
                foreach (BaseCardSectionRow row in document.GetSection(CardDocument.ReceiversPartners.ID)) {
                    Guid partnersId = Guid.Empty;
                    if (Guid.TryParse(row["ReceiverPartnerCo"] + "", out partnersId)) {
                        PartnersCompany partner = context.ObjectContext.GetObject<PartnersCompany>(partnersId);
                        if (partner != null)
                            partners.Add(partner);
                    }
                }
                return partners.Select(x => x.Name).ToArray();
            }
        }
    }
}