using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.WinControls.UI.Localization;
using Telerik.WinControls;

namespace PU.Classes
{
        public class MyRussianRadGridLocalizationProvider : RadGridLocalizationProvider
        {
            public override string GetLocalizedString(string id)
            {
                switch (id)
                {
                    case RadGridStringId.GroupingPanelDefaultMessage: return "Перетащите сюда колонку для группировки по ней";
                    case RadGridStringId.AddNewRowString: return "Добавить новую запись";
                    case RadGridStringId.ClearValueMenuItem: return "Очистить значение";
                    case RadGridStringId.CopyMenuItem: return "Копировать";
                    case RadGridStringId.CutMenuItem: return "Вырезать";
                    case RadGridStringId.DeleteRowMenuItem: return "Удалить запись";
                    case RadGridStringId.EditMenuItem: return "Редактировать";
                    case RadGridStringId.NoDataText: return "Нет данных";
                    case RadGridStringId.HideMenuItem: return "Скрыть";
                    case RadGridStringId.PasteMenuItem: return "Вставить";

                    case RadGridStringId.ConditionalFormattingInvalidParameters: return "Неверные параметры";
                    case RadGridStringId.FilterFunctionBetween: return "Между";
                    case RadGridStringId.FilterFunctionContains: return "Содержит";
                    case RadGridStringId.FilterFunctionDoesNotContain: return "Не содержит";
                    case RadGridStringId.FilterFunctionEndsWith: return "Оканчивается на";
                    case RadGridStringId.FilterFunctionEqualTo: return "Равно";
                    case RadGridStringId.FilterFunctionGreaterThan: return "Больше чем";
                    case RadGridStringId.FilterFunctionGreaterThanOrEqualTo: return "Больше или равно";
                    case RadGridStringId.FilterFunctionIsEmpty: return "Пусто";
                    case RadGridStringId.FilterFunctionIsNull: return "Null";
                    case RadGridStringId.FilterFunctionLessThan: return "Меньше чем";
                    case RadGridStringId.FilterFunctionLessThanOrEqualTo: return "Меньше или равно";
                    case RadGridStringId.FilterFunctionNoFilter: return "Без фильтра";
                    case RadGridStringId.FilterFunctionNotBetween: return "Не между";
                    case RadGridStringId.FilterFunctionNotEqualTo: return "Не равно";
                    case RadGridStringId.FilterFunctionNotIsEmpty: return "Не пусто";
                    case RadGridStringId.FilterFunctionNotIsNull: return "Не Null";
                    case RadGridStringId.FilterFunctionStartsWith: return "Начинается с";
                    case RadGridStringId.FilterFunctionCustom: return "Произвольный";

                    case RadGridStringId.FilterOperatorBetween: return "Между";
                    case RadGridStringId.FilterOperatorContains: return "Содержит";
                    case RadGridStringId.FilterOperatorDoesNotContain: return "Не содержит";
                    case RadGridStringId.FilterOperatorEndsWith: return "Оканчивается на";
                    case RadGridStringId.FilterOperatorEqualTo: return "Равно";
                    case RadGridStringId.FilterOperatorGreaterThan: return "Больше чем";
                    case RadGridStringId.FilterOperatorGreaterThanOrEqualTo: return "Больше чем или равно";
                    case RadGridStringId.FilterOperatorIsEmpty: return "Пусто";
                    case RadGridStringId.FilterOperatorIsNull: return "Null";
                    case RadGridStringId.FilterOperatorLessThan: return "Меньше чем";
                    case RadGridStringId.FilterOperatorLessThanOrEqualTo: return "Меньше чем или равно";
                    case RadGridStringId.FilterOperatorNoFilter: return "Без фильтра";
                    case RadGridStringId.FilterOperatorNotBetween: return "Не между";
                    case RadGridStringId.FilterOperatorNotEqualTo: return "Не равно";
                    case RadGridStringId.FilterOperatorNotIsEmpty: return "Не пусто";
                    case RadGridStringId.FilterOperatorNotIsNull: return "Не Null";
                    case RadGridStringId.FilterOperatorStartsWith: return "Начинается с";
                    case RadGridStringId.FilterOperatorIsLike: return "Совпадает";
                    case RadGridStringId.FilterOperatorNotIsLike: return "Не совпадает";
                    case RadGridStringId.FilterOperatorIsContainedIn: return "Содержится в";
                    case RadGridStringId.FilterOperatorNotIsContainedIn: return "Не содержится в";
                    case RadGridStringId.FilterOperatorCustom: return "Произвольный";

                    case RadGridStringId.CustomFilterMenuItem: return "Произвольный";
                    case RadGridStringId.CustomFilterDialogCaption: return "Окно фильтров таблицы [{0}]";
                    case RadGridStringId.CustomFilterDialogLabel: return "Показать строки где:";
                    case RadGridStringId.CustomFilterDialogRbAnd: return "И";
                    case RadGridStringId.CustomFilterDialogRbOr: return "Или";
                    case RadGridStringId.CustomFilterDialogBtnOk: return "OK";
                    case RadGridStringId.CustomFilterDialogBtnCancel: return "Закрыть";
                    case RadGridStringId.CustomFilterDialogCheckBoxNot: return "Нет";
                    case RadGridStringId.CustomFilterDialogTrue: return "True";
                    case RadGridStringId.CustomFilterDialogFalse: return "False";

                    case RadGridStringId.FilterMenuAvailableFilters: return "Доступные фильтры";
                    case RadGridStringId.FilterMenuSearchBoxText: return "Поиск...";
                    case RadGridStringId.FilterMenuClearFilters: return "Очистить";
                    case RadGridStringId.FilterMenuButtonOK: return "OK";
                    case RadGridStringId.FilterMenuButtonCancel: return "Закрыть";
                    case RadGridStringId.FilterMenuSelectionAll: return "Все";
                    case RadGridStringId.FilterMenuSelectionAllSearched: return "Все результаты";
                    case RadGridStringId.FilterMenuSelectionNull: return "Null";
                    case RadGridStringId.FilterMenuSelectionNotNull: return "Не Null";

                    case RadGridStringId.FilterFunctionSelectedDates: return "Фильтр по датам:";
                    case RadGridStringId.FilterFunctionToday: return "Сегодня";
                    case RadGridStringId.FilterFunctionYesterday: return "Вчера";
                    case RadGridStringId.FilterFunctionDuringLast7days: return "Последние 7 дней";

                    case RadGridStringId.FilterLogicalOperatorAnd: return "И";
                    case RadGridStringId.FilterLogicalOperatorOr: return "ИЛИ";
                    case RadGridStringId.FilterCompositeNotOperator: return "НЕ";

                    case RadGridStringId.SortAscendingMenuItem: return "Сортировать по возрастанию";
                    case RadGridStringId.SortDescendingMenuItem: return "Сортировать по убыванию";
                    case RadGridStringId.ClearSortingMenuItem: return "Без сортировки";
                    case RadGridStringId.ConditionalFormattingMenuItem: return "Условное форматирование";
                    case RadGridStringId.GroupByThisColumnMenuItem: return "Группировать по этой колонке";
                    case RadGridStringId.UngroupThisColumn: return "Разгруппировать эту колонку";
                    case RadGridStringId.ColumnChooserMenuItem: return "Выбор колонок";
                    case RadGridStringId.HideGroupMenuItem: return "Скрыть группу";
                    case RadGridStringId.UnpinMenuItem: return "Открепить колонку";
                    case RadGridStringId.UnpinRowMenuItem: return "Открепить строку";
                    case RadGridStringId.PinMenuItem: return "Закрепленные состояния";
                    case RadGridStringId.PinAtLeftMenuItem: return "Закрепить слева";
                    case RadGridStringId.PinAtRightMenuItem: return "Закрепить справа";
                    case RadGridStringId.PinAtBottomMenuItem: return "Закрепить снизу";
                    case RadGridStringId.PinAtTopMenuItem: return "Закрепить сверху";
                    case RadGridStringId.BestFitMenuItem: return "Подогнать размер";
                    case RadGridStringId.SearchRowResultsOfLabel: return "из";
                    case RadGridStringId.SearchRowMatchCase: return "Учитывать регистр";
                    case RadGridStringId.ConditionalFormattingSortAlphabetically: return "Сортировать колонки по алфавиту";
                    case RadGridStringId.ConditionalFormattingCaption: return "Настройки условного форматирования";
                    case RadGridStringId.ConditionalFormattingLblColumn: return "Форматировать только ячейки с";
                    case RadGridStringId.ConditionalFormattingLblName: return "Название правила";
                    case RadGridStringId.ConditionalFormattingLblType: return "Значение ячейки";
                    case RadGridStringId.ConditionalFormattingLblValue1: return "Значение 1";
                    case RadGridStringId.ConditionalFormattingLblValue2: return "Значение 2";
                    case RadGridStringId.ConditionalFormattingGrpConditions: return "Правила";
                    case RadGridStringId.ConditionalFormattingGrpProperties: return "Параметры правила";
                    case RadGridStringId.ConditionalFormattingChkApplyToRow: return "Применить форматирование ко всей строке";
                    case RadGridStringId.ConditionalFormattingChkApplyOnSelectedRows: return "Применить форматирование если строка выбрана";
                    case RadGridStringId.ConditionalFormattingBtnAdd: return "Добавить";
                    case RadGridStringId.ConditionalFormattingBtnRemove: return "Удалить";
                    case RadGridStringId.ConditionalFormattingBtnOK: return "OK";
                    case RadGridStringId.ConditionalFormattingBtnCancel: return "Закрыть";
                    case RadGridStringId.ConditionalFormattingBtnApply: return "Применить";
                    case RadGridStringId.ConditionalFormattingRuleAppliesOn: return "Правило применяется к";
                    case RadGridStringId.ConditionalFormattingCondition: return "Условие";
                    case RadGridStringId.ConditionalFormattingExpression: return "Выражение";
                    case RadGridStringId.ConditionalFormattingChooseOne: return "[Выбрать]";
                    case RadGridStringId.ConditionalFormattingEqualsTo: return "Равно [Value1]";
                    case RadGridStringId.ConditionalFormattingIsNotEqualTo: return "Не равно [Value1]";
                    case RadGridStringId.ConditionalFormattingStartsWith: return "Начинается с [Value1]";
                    case RadGridStringId.ConditionalFormattingEndsWith: return "Оканчивается на [Value1]";
                    case RadGridStringId.ConditionalFormattingContains: return "Содержит [Value1]";
                    case RadGridStringId.ConditionalFormattingDoesNotContain: return "Не содержит [Value1]";
                    case RadGridStringId.ConditionalFormattingIsGreaterThan: return "Больше чем [Value1]";
                    case RadGridStringId.ConditionalFormattingIsGreaterThanOrEqual: return "Больше или равно [Value1]";
                    case RadGridStringId.ConditionalFormattingIsLessThan: return "Меньше чем [Value1]";
                    case RadGridStringId.ConditionalFormattingIsLessThanOrEqual: return "Меньше или равно [Value1]";
                    case RadGridStringId.ConditionalFormattingIsBetween: return "Между [Value1] и [Value2]";
                    case RadGridStringId.ConditionalFormattingIsNotBetween: return "Не между [Value1] и [Value1]";
                    case RadGridStringId.ConditionalFormattingLblFormat: return "Формат";

                    case RadGridStringId.ConditionalFormattingBtnExpression: return "Редактор выражений";
                    case RadGridStringId.ConditionalFormattingTextBoxExpression: return "Выражение";

                    case RadGridStringId.ConditionalFormattingPropertyGridCaseSensitive: return "Чувствительный к регистру";
                    case RadGridStringId.ConditionalFormattingPropertyGridCellBackColor: return "Фон ячейки";
                    case RadGridStringId.ConditionalFormattingPropertyGridCellForeColor: return "Цвет ячейки";
                    case RadGridStringId.ConditionalFormattingPropertyGridEnabled: return "Включен";
                    case RadGridStringId.ConditionalFormattingPropertyGridRowBackColor: return "Фон строки";
                    case RadGridStringId.ConditionalFormattingPropertyGridRowForeColor: return "Цвет строки";
                    case RadGridStringId.ConditionalFormattingPropertyGridRowTextAlignment: return "Выравнивание в строке";
                    case RadGridStringId.ConditionalFormattingPropertyGridTextAlignment: return "Выравнивание";

                    case RadGridStringId.ConditionalFormattingPropertyGridCaseSensitiveDescription: return "Determines whether case-sensitive comparisons will be made when evaluating string values.";
                    case RadGridStringId.ConditionalFormattingPropertyGridCellBackColorDescription: return "Enter the background color to be used for the cell.";
                    case RadGridStringId.ConditionalFormattingPropertyGridCellForeColorDescription: return "Enter the foreground color to be used for the cell.";
                    case RadGridStringId.ConditionalFormattingPropertyGridEnabledDescription: return "Determines whether the condition is enabled (can be evaluated and applied).";
                    case RadGridStringId.ConditionalFormattingPropertyGridRowBackColorDescription: return "Enter the background color to be used for the entire row.";
                    case RadGridStringId.ConditionalFormattingPropertyGridRowForeColorDescription: return "Enter the foreground color to be used for the entire row.";
                    case RadGridStringId.ConditionalFormattingPropertyGridRowTextAlignmentDescription: return "Enter the alignment to be used for the cell values, when ApplyToRow is true.";
                    case RadGridStringId.ConditionalFormattingPropertyGridTextAlignmentDescription: return "Enter the alignment to be used for the cell values.";

                    case RadGridStringId.ColumnChooserFormCaption: return "Выбор колонок";
                    case RadGridStringId.ColumnChooserFormMessage: return "Перетащите заголовок столбца\nсюда или отсюда\nчтобы отобразить его.";
                    case RadGridStringId.GroupingPanelHeader: return "Группировать по:";
                    case RadGridStringId.PagingPanelPagesLabel: return "Страница";
                    case RadGridStringId.PagingPanelOfPagesLabel: return "из";
                    case RadGridStringId.CompositeFilterFormErrorCaption: return "Ошибка фильтра";
                    case RadGridStringId.CompositeFilterFormInvalidFilter: return "Параметры фильтра заданы неверно.";

                    case RadGridStringId.ExpressionMenuItem: return "Выражение";
                    case RadGridStringId.ExpressionFormTitle: return "Построитель выражений";
                    case RadGridStringId.ExpressionFormFunctions: return "Функции";
                    case RadGridStringId.ExpressionFormFunctionsText: return "Текст";
                    case RadGridStringId.ExpressionFormFunctionsAggregate: return "Совокупные";
                    case RadGridStringId.ExpressionFormFunctionsDateTime: return "Дата-время";
                    case RadGridStringId.ExpressionFormFunctionsLogical: return "Логические";
                    case RadGridStringId.ExpressionFormFunctionsMath: return "Математические";
                    case RadGridStringId.ExpressionFormFunctionsOther: return "Другие";
                    case RadGridStringId.ExpressionFormOperators: return "Операторы";
                    case RadGridStringId.ExpressionFormConstants: return "Константы";
                    case RadGridStringId.ExpressionFormFields: return "Поля";
                    case RadGridStringId.ExpressionFormDescription: return "Описание";
                    case RadGridStringId.ExpressionFormResultPreview: return "Предпросмотр результата";
                    case RadGridStringId.ExpressionFormTooltipPlus: return "Плюс";
                    case RadGridStringId.ExpressionFormTooltipMinus: return "Минус";
                    case RadGridStringId.ExpressionFormTooltipMultiply: return "Умножить";
                    case RadGridStringId.ExpressionFormTooltipDivide: return "Делить";
                    case RadGridStringId.ExpressionFormTooltipModulo: return "Модуль";
                    case RadGridStringId.ExpressionFormTooltipEqual: return "Равно";
                    case RadGridStringId.ExpressionFormTooltipNotEqual: return "Не равно";
                    case RadGridStringId.ExpressionFormTooltipLess: return "Меньше";
                    case RadGridStringId.ExpressionFormTooltipLessOrEqual: return "Меньше или равно";
                    case RadGridStringId.ExpressionFormTooltipGreaterOrEqual: return "Больше или равно";
                    case RadGridStringId.ExpressionFormTooltipGreater: return "Больше";
                    case RadGridStringId.ExpressionFormTooltipAnd: return "Логическое \"И\"";
                    case RadGridStringId.ExpressionFormTooltipOr: return "Логическое \"ИЛИ\"";
                    case RadGridStringId.ExpressionFormTooltipNot: return "Логическое \"НЕ\"";
                    case RadGridStringId.ExpressionFormAndButton: return string.Empty; //if empty, default button image is used
                    case RadGridStringId.ExpressionFormOrButton: return string.Empty; //if empty, default button image is used
                    case RadGridStringId.ExpressionFormNotButton: return string.Empty; //if empty, default button image is used
                    case RadGridStringId.ExpressionFormOKButton: return "OK";
                    case RadGridStringId.ExpressionFormCancelButton: return "Закрыть";
                }
                
                return string.Empty;
            }
        }


        public class RussianRadMessageBoxLocalizationProvider : RadMessageLocalizationProvider
        {
            public override string GetLocalizedString(string id)
            {
                switch (id)
                {
                    case RadMessageStringID.AbortButton:
                        return "Отменить";
                    case RadMessageStringID.CancelButton:
                        return "Отмена";
                    case RadMessageStringID.IgnoreButton:
                        return "Игнорировать";
                    case RadMessageStringID.NoButton:
                        return "Нет";
                    case RadMessageStringID.OKButton:
                        return "OK";
                    case RadMessageStringID.RetryButton:
                        return "Повторить";
                    case RadMessageStringID.YesButton:
                        return "Да";
                }
                return string.Empty;
            }
        }

}
