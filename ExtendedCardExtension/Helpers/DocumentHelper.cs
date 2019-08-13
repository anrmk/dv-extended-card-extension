using DocsVision.BackOffice.CardLib.CardDefs;
using DocsVision.BackOffice.ObjectModel;
using DocsVision.BackOffice.ObjectModel.Services;
using DocsVision.Platform.ObjectManager;
using DocsVision.Platform.ObjectManager.Metadata;
using DocsVision.Platform.ObjectManager.SearchModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExtendedCardExtension.Helpers {
    public class DocumentHelper {
        private readonly Document document;
        private readonly SessionContext context;
        private IList contractSection;
        private IList controlSection;

        public DocumentHelper(Document doc, SessionContext context) {
            this.context = context;
            if (doc != null)
                document = doc;
        }

        /// <summary>
        /// Получить серверное время
        /// </summary>
        public DateTime GetServerDateTime => (DateTime)context.Session.Properties["ServerTime"].Value;

        public Document GetDocument => document;

        /// <summary>
        /// Секция "Основная информация"
        /// </summary>
        public DocumentMainInfo GetMainInfo => GetDocument?.MainInfo;

        /// <summary>
        /// Секция "Системная информация"
        /// </summary>
        public BaseCardSystemInfo SystemInfo => GetDocument?.SystemInfo;

        private BaseCardSectionRow ContractSection {
            get {
                if (contractSection == null) {
                    contractSection = GetDocument.GetSection(CardDocument.Contract.ID);
                }
                if (contractSection != null && contractSection.Count > 0) {
                    return (BaseCardSectionRow)contractSection[0];
                } else {
                    return new BaseCardSectionRow();
                }
            }
        }

        private BaseCardSectionRow ControlSection {
            get {
                if (controlSection == null) {
                    controlSection = GetDocument.GetSection(CardDocument.Control.ID);
                }
                if (controlSection != null && controlSection.Count > 0) {
                    return (BaseCardSectionRow)controlSection[0];
                } else {
                    return new BaseCardSectionRow();
                }
            }
        }

        /// <summary>
        /// Индентификатор карточки
        /// </summary>
        public Guid Id => GetDocument.GetObjectId();

        /// <summary>
        /// Дата создания документа
        /// </summary>
        public string CreatedDate => GetDocument.CreateDate.ToString("dd.MM.yyyy HH:mm:ss");

        /// <summary>
        /// Наименование (предмет договора)
        /// </summary>
        public string Name => GetMainInfo.Name;

        /// <summary>
        /// Вид заявки
        /// </summary>
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

                var baseUniversal = GetDocument.GetSection(new Guid("2A157825-4B9C-44B1-B0A5-10364A502944"));
                if (baseUniversal != null) {
                    foreach (BaseCardSectionRow tableRow in baseUniversal) {
                        Guid itemId = Guid.Empty;
                        if (Guid.TryParse(tableRow["Equipment"] as string, out itemId)) {
                            BaseUniversalItem item = this.context.ObjectContext.GetObject<BaseUniversalItem>(itemId);
                            if (item != null)
                                description += $", {item.Name}";
                        }
                    }
                }

                //определение пятна
                if (!string.IsNullOrEmpty(ControlSection["String2"] as string))
                    description += $"Пятно: {ControlSection["String2"]}";

                return description;
            }
        }

        /// <summary>
        /// Содержание/Описание
        /// </summary>
        public string Content => GetMainInfo[CardDocument.MainInfo.Content] as string;

        /// <summary>
        /// Наименование Документа, номер и дата
        /// </summary>
        public string DocumentName => string.Format($"{SystemInfo?.CardKind.Name} {0}", GetDocument?.Numbers.Select(x => $"№ {x.Number} от { GetMainInfo.DeliveryDate.ToShortDateString() }").FirstOrDefault());

        public string RegistrarName => GetMainInfo.Registrar?.DisplayName;
        public string AuthorName => GetMainInfo.Author?.DisplayName;

        /// <summary>
        /// Получение списка исполнителей
        /// </summary>
        public List<StaffEmployee> AcquaintanceStaff {
            get {
                var result = new List<StaffEmployee>();
                var section = GetDocument.GetSection(CardDocument.AcquaintanceStaff.ID);
                if (section != null) {
                    foreach (BaseCardSectionRow sectionRow in section) {
                        Guid guid = Guid.Empty;
                        if (Guid.TryParse(sectionRow["AcquaintancePersons"] as string, out guid)) {
                            StaffEmployee item = this.context.ObjectContext.GetObject<StaffEmployee>(guid);
                            if (item != null) {
                                result.Add(item);
                            }
                        }
                    }
                }
                return result;
            }
        }
        /// <summary>
        /// Получение текущего исполнителя
        /// </summary>
        public string CurrentAcquaintanceStaff {
            get {
                List<StaffEmployee> staffEmployee = AcquaintanceStaff;
                if (staffEmployee.Count > 0) {
                    return staffEmployee[0].ShortName;
                }
                return string.Empty;
                //var performers = "";
                //var section = GetDocument.GetSection(CardDocument.AcquaintanceStaff.ID); // получение секции по исполнителю (действующие лица)
                //if (section != null) {
                //    var sectionRow = section.Cast<BaseCardSectionRow>()?.FirstOrDefault(i => i != null);
                //    Guid guid = Guid.Empty;
                //    if (Guid.TryParse(sectionRow["AcquaintancePersons"] as string, out guid)) {
                //        StaffEmployee item = this.context.ObjectContext.GetObject<StaffEmployee>(guid);
                //        if (item != null)
                //            performers += $"{item.ShortName}; ";
                //    }
                //}
                //return performers;
            }
        }

        /// <summary>
        /// Заказчик
        /// </summary>
        public string ResponseStaffName {
            get {
                Guid guid = Guid.Empty;
                if (Guid.TryParse(GetMainInfo[CardDocument.MainInfo.ResponsDepartment] as string, out guid)) {
                    StaffUnit unit = context.ObjectContext.GetObject<StaffUnit>(guid);
                    return unit.Name;
                }
                return "";
            }
        }

        /// <summary>
        /// Определение сторон
        /// </summary>
        public List<PartnersCompany> PartnersCompany {
            get {
                List<PartnersCompany> partners = new List<PartnersCompany>();
                foreach (BaseCardSectionRow row in GetDocument.GetSection(CardDocument.ReceiversPartners.ID)) {
                    Guid guid = Guid.Empty;
                    if (Guid.TryParse(row["ReceiverPartnerCo"] as string, out guid)) {
                        PartnersCompany partner = context.ObjectContext.GetObject<PartnersCompany>(guid);
                        if (partner != null)
                            partners.Add(partner);
                    }
                }
                return partners;
            }
        }

        /// <summary>
        /// Сумма контракта
        /// </summary>
        public string ContractAmount {
            get {
                var result = ContractSection["Sum"] as string;
                double sum = 0.0;
                if (double.TryParse(result, out sum)) {
                    Math.Round(sum, 2, MidpointRounding.AwayFromZero);
                    result = sum.ToString();
                }
                switch (ContractSection["ContractCurrency"] as string) {
                    case "15":
                        result += " KZT";
                        break;
                    case "2":
                        result += " USD";
                        break;
                    case "0":
                        result += " EUR";
                        break;
                    default:
                        break;
                }
                return result;
            }
        }

        /// <summary>
        /// Бюджет
        /// </summary>
        public string ContractTotalAmount => ContractSection["ContractTotalSum"] as string;

        /// <summary>
        /// Сметная стоимость
        /// </summary>
        public string EstimatedCost => ContractSection["ContractSum"] as string;

        /// <summary>
        /// Трудозатраты
        /// </summary>
        public double Labourness {
            get {
                double labourness = 0.0;
                double.TryParse(ContractSection["Labourness"] as string, out labourness);
                return labourness;
            }
        }

        /// <summary>
        /// Аванс
        /// </summary>
        public string Deposit => GetMainInfo[CardDocument.MainInfo.ExternalNumber] as string;

        /// <summary>
        /// Особые условия
        /// </summary>
        public string ContractNotes => $"{ContractSection["ContractNotes"] as string} {ContractSection["AttachmentNumber"] as string}".Trim();

        #region Определение срока выполнения работ
        /// <summary>
        /// Срок выполнения начало
        /// </summary>
        public DateTime ContractBegin {
            get {
                DateTime dateTime;
                DateTime.TryParse(ContractSection["ContractBegin"] as string, out dateTime);
                return dateTime;
            }
        }

        /// <summary>
        /// Срок выполнения окончание
        /// </summary>
        public DateTime ContractEnd {
            get {
                DateTime dateTime;
                DateTime.TryParse(ContractSection["ContractEnd"] as string, out dateTime);
                return dateTime;
            }
        }

        /// <summary>
        /// Примечания по срокам
        /// </summary>
        public string DeadlineNotes => ControlSection["StringField"] as string;
        #endregion

        /// <summary>
        /// Гарантия
        /// </summary>
        public string Guarantee {
            get {
                var result = ContractSection["ContractSubject"] as string;
                bool wasSent;
                bool.TryParse(GetMainInfo[CardDocument.MainInfo.WasSent] as string, out wasSent);
                if (!string.IsNullOrEmpty(result)) {
                    result += wasSent ? " (с обеспечением)" : " (без обеспечения)";
                }
                return result;
            }
        }

        //public bool SaveCard {
        //    get {
        //        bool result = false;
        //        MethodInfo saveCardMethodInfo = typeof(BaseCardControl).GetMethod("SaveCard", BindingFlags.NonPublic | BindingFlags.Instance);
        //        if (saveCardMethodInfo != null)
        //            result = (bool)saveCardMethodInfo.Invoke(document, new object[] { false });

        //        return result;
        //    }
        //}

        public void ChangeStatus(string state) {
            IStateService StateService = context.ObjectContext.GetService<IStateService>();
            IList<StatesStateMachineBranch> statesStateMachineBranch = StateService.FindLineBranchesByStartState(SystemInfo.State).ToList();

            StatesStateMachineBranch branch = statesStateMachineBranch
                .Where(t => t.StartState == SystemInfo.State 
                    && t.BranchType == StatesStateMachineBranchBranchType.Line
                    && t.EndState.DefaultName.Equals(state)).FirstOrDefault();

            if (branch != null) {
                //смена статуса
                StateService.ChangeState(GetDocument, branch);
            } else {
                throw new Exception("Переход в состояние невозможен, так как данное состояние отсутствует");
            }
        }

        /// <summary>
        /// Получение списка карточек по типу
        /// </summary>
        public List<CardData> Cards {
            get {
                SearchQuery searchQuery = context.Session.CreateSearchQuery();
                //Поиск по типу карточки
                CardTypeQuery typeQuery = searchQuery.AttributiveSearch.CardTypeQueries.AddNew(new Guid("9C0E5586-41B8-411E-B5CD-C94B605CB7A1"));
                //Поиск по секции
                SectionQuery sectionQuery = typeQuery.SectionQueries.AddNew(new Guid("35473281-BCEB-415A-8603-74549421037E"));
                //Поиск по значению поля
                sectionQuery.ConditionGroup.Conditions.AddNew("Document", FieldType.RefCardId, ConditionOperation.Equals, this.Id /* context.ObjectContext.GetObjectRef<Document>(GetDocument).Id*/);
                //Получение текста запроса
                string query = searchQuery.GetXml();
                //Выполнение запроса
                CardDataCollection coll = context.Session.CardManager.FindCards(query);

                return coll.ToList();
            }
        }
    }
}