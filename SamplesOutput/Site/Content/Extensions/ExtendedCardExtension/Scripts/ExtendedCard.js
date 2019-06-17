var WebClient;
(function (WebClient) {
    var ExtendedCard = /** @class */ (function () {
        function ExtendedCard() {
            var args = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                args[_i] = arguments[_i];
            }
        }
        ExtendedCard.prototype.extendedCardTasksRender = function (data) {
            return (React.createElement("div", { className: "show-report-data" },
                React.createElement("div", null,
                    React.createElement("label", null,
                        "\u0422\u0435\u043C\u0430: ",
                        data.sortName)),
                React.createElement("div", null,
                    React.createElement("label", null,
                        "\u0410\u0432\u0442\u043E\u0440: ",
                        data.author)),
                React.createElement("div", null,
                    React.createElement("label", null,
                        "\u0422\u0435\u043A\u0443\u0449\u0438\u0439 \u0438\u0441\u043F\u043E\u043B\u043D\u0438\u0442\u0435\u043B\u044C: ",
                        data.currentPerformers)),
                React.createElement("div", null,
                    React.createElement("label", null,
                        "\u0421\u043E\u0441\u0442\u043E\u044F\u043D\u0438\u0435 \u0434\u043E\u043A\u0443\u043C\u0435\u043D\u0442\u0430: ",
                        data.state)),
                React.createElement("div", null,
                    React.createElement("table", null,
                        React.createElement("caption", null, "\u0417\u0430\u0434\u0430\u043D\u0438\u044F \u043F\u043E \u0434\u043E\u043A\u0443\u043C\u0435\u043D\u0442\u044B:"),
                        React.createElement("thead", null,
                            React.createElement("tr", null,
                                React.createElement("th", null, "\u0421\u0441\u044B\u043B\u043A\u0430 \u043D\u0430 \u0437\u0430\u0434\u0430\u043D\u0438\u0435"),
                                React.createElement("th", null, "\u0410\u0432\u0442\u043E\u0440"),
                                React.createElement("th", null, "\u0421\u043E\u0441\u0442\u043E\u044F\u043D\u0438\u0435"),
                                React.createElement("th", null, "\u0414\u0430\u0442\u0430 \u0437\u0430\u0432\u0435\u0440\u0448\u0435\u043D\u0438\u044F (\u043F\u043B\u0430\u043D\u043E\u0432\u0430\u044F)"),
                                React.createElement("th", null, "\u041D\u0430\u0437\u043D\u0430\u0447\u0435\u043D\u043D\u044B\u0439 \u0438\u0441\u043F\u043E\u043B\u043D\u0438\u0442\u0435\u043B\u044C"),
                                React.createElement("th", null, "\u0422\u0435\u043A\u0443\u0449\u0438\u0439 \u0438\u0441\u043F\u043E\u043B\u043D\u0438\u0442\u0435\u043B\u044C"))),
                        React.createElement("tbody", null, data.childList.map(function (value, index) { return React.createElement("tr", { key: index },
                            React.createElement("td", null,
                                React.createElement("a", { href: "#/TaskView/" + value.id, target: '_blank' }, value.id)),
                            React.createElement("td", null, value.author),
                            React.createElement("td", null, value.state),
                            React.createElement("td", null, value.endDate),
                            React.createElement("td", null, value.performers),
                            React.createElement("td", null, value.currentPerformers)); }))))));
        };
        /**
         * Получить информацию по заданиям
         * @param cardId - идентификатор документа
         */
        ExtendedCard.prototype.getExtendedCardTasks = function (cardId) {
            var url = urlStore.urlResolver.resolveApiUrl("GetExtendedCardTasks", "ExtendedCard");
            return requestManager.get(url + "?cardId=" + cardId);
        };
        ExtendedCard.prototype.extendedCarStatusLogsRender = function (data) {
            return (React.createElement("div", { className: "show-report-data" },
                React.createElement("div", null,
                    React.createElement("label", null,
                        "\u041D\u0430\u0437\u0432\u0430\u043D\u0438\u0435: ",
                        data.sortName)),
                React.createElement("div", null,
                    React.createElement("label", null,
                        "\u0414\u0430\u0442\u0430 \u0441\u043E\u0437\u0434\u0430\u043D\u0438\u044F: ",
                        data.sortName)),
                React.createElement("div", null,
                    React.createElement("label", null,
                        "\u0410\u0432\u0442\u043E\u0440: ",
                        data.author)),
                React.createElement("div", null,
                    React.createElement("label", null,
                        "\u0421\u043E\u0441\u0442\u043E\u044F\u043D\u0438\u0435 \u0434\u043E\u043A\u0443\u043C\u0435\u043D\u0442\u0430: ",
                        data.state)),
                React.createElement("div", null,
                    React.createElement("table", null,
                        React.createElement("caption", null, "\u0418\u043D\u0444\u043E\u0440\u043C\u0430\u0446\u0438\u044F \u043F\u043E \u043F\u0435\u0440\u0435\u0445\u043E\u0434\u0443 \u0441\u043E\u0441\u0442\u043E\u044F\u043D\u0438\u0439:"),
                        React.createElement("thead", null,
                            React.createElement("tr", null,
                                React.createElement("th", null, "\u0421\u043E\u0442\u0440\u0443\u0434\u043D\u0438\u043A"),
                                React.createElement("th", null, "\u041F\u0435\u0440\u0435\u0445\u043E\u0434 \u0432 \u0441\u043E\u0441\u0442\u043E\u044F\u043D\u0438\u0435"),
                                React.createElement("th", null, "\u041D\u0430\u0447\u0430\u043B\u043E \u044D\u0442\u0430\u043F\u0430"),
                                React.createElement("th", null, "\u041E\u043A\u043E\u043D\u0447\u0430\u043D\u0438\u0435 \u044D\u0442\u0430\u043F\u0430"),
                                React.createElement("th", null, "\u0417\u0430\u0442\u0440\u0430\u0447\u0435\u043D\u043E \u0432\u0440\u0435\u043C\u0435\u043D\u0438"))),
                        React.createElement("tbody", null, data.childList.map(function (value, index) { return React.createElement("tr", { key: index },
                            React.createElement("td", null, value.employeeName),
                            React.createElement("td", null, value.workLabel),
                            React.createElement("td", null, value.date),
                            React.createElement("td", null, value.endDate),
                            React.createElement("td", null, value.labourness)); }))))));
        };
        /**
         * Журнал перехода состояний
         * @param cardId - идентификатор документа
         */
        ExtendedCard.prototype.getCardStatusLogs = function (cardId) {
            var url = urlStore.urlResolver.resolveApiUrl("GetCardStatusLogs", "ExtendedCard");
            return requestManager.get(url + "?cardId=" + cardId);
        };
        ExtendedCard.prototype.getCardKindId = function (cardId) {
            var url = urlStore.urlResolver.resolveApiUrl("GetCardKindId", "ExtendedCard");
            return requestManager.get(url + "?cardId=" + cardId);
        };
        return ExtendedCard;
    }());
    WebClient.ExtendedCard = ExtendedCard;
})(WebClient || (WebClient = {}));
function getExtendedCardTasks(sender) {
    var layout = sender.layout;
    var srd = new WebClient.ExtendedCard();
    srd.getExtendedCardTasks(layout.cardInfo.id).then(function (response) {
        if (response != null) {
            var element = srd.extendedCardTasksRender(response);
            WebClient.MessageBox.ShowInfo(element, "" + response.sortName).done(function () {
                //alert("Диалог закрыт");
            });
        }
    });
}
function getExtendedCardStatusLogs(sender) {
    var layout = sender.layout;
    var srd = new WebClient.ExtendedCard();
    srd.getCardStatusLogs(layout.cardInfo.id).then(function (response) {
        if (response != null) {
            var element = srd.extendedCarStatusLogsRender(response);
            WebClient.MessageBox.ShowInfo(element, "" + response.sortName).done(function () { });
        }
    });
}
