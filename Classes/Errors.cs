using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NotificationsService.Classes
{

    public static class Errors
    {
        public const int FillData = 1;
        public const int NoPermissions = 2;
        public const int UsernameNotAvailable = 3;
        public const int MobileNumberIsNotAvailable = 4;
        public const int UsernameLengthMustBeEightOrOverCharacters = 5;
        public const int SenderCannotBeReceiverAtTheSameTime = 6;
        public const int NoSearchResult = 7;
        public const int QRCodeIsNotAvailable = 8;
        public const int CannotTransferToAdminUsers = 9;
        public const int NoBalance = 10;
        public const int CannotTransferPointsToThisTypeOfUsers = 11;
        public const int DataValidationError = 12;
        public const int IncorrectUsernameOrPassword = 13;
        public const int NegativeNumber = 14;
        public const int DuplicatedData = 15;
        public const int CantRenewItemHold = 16;
        public const int QuantityMustBeBiggerThanZero = 17;
        public const int HoldPeriodMustBeBiggerThanOne = 18;
        public const int OrderHoldPeriodMustBeBiggerThanItemHoldPeriod = 19;
        public const int CannotChangeUserFollowPermissionFromTemplate = 20;
        public const int CannotCancelOrder = 21;
        public const int OfferItemShouldHaveInitQuantity = 22;
        public const int CannotSubmitEmptyOrder = 23;
        public const int CannotUpdateOfferItem = 24;
        public const int CannotTransferToDisabledUser = 25;
        public const int CannotCancelOrderInThisStatus = 26;
        public const int CannotAddItemToOrderInThisStatus = 27;

        public const int UnknownError = -1;
        public const int UnknownErrorABC = 0;
        public const int NotValidAccessToken = 100;

        public const int UserIsDisabled = 200;
        public const int UserIsNotActive = 300;



        public const string FillDataArabicMessage = "يرجى ملئ البيانات";
        public const string NoPermissionsCodeArabicMessage = "لا يوجد صلاحيات لإتمام هذه العملية";
        public const string UsernameNotAvailableArabicMessage = "اسم المستخدم غير متاح";
        public const string MobileNumberIsNotAvailableArabicMessage = "رقم الهاتف مستخدم من قبل";
        public const string UsernameLengthMustBeEightOrOverCharactersArabicMessage = "اسم المستخدم يجب أن يكون 8 أحرف أو اكثر";
        public const string SenderCannotBeReceiverAtTheSameTimeArabicMessage = "لا يمكن أن يكون المرسل هو نفسه المستقبل";
        public const string NoBalanceErrorArabicMessage = "لا يوجد رصيد كافي لإتمام العملية";
        public const string CannotTransferToAdminUsersArabicMessage = "لا يمكن تحويل رصيد إلى مستخدمين نظام أو مدراء نظام";
        public const string CannotTransferPointsToThisTypeOfUsersArabicMessage = "لا يمكن تحويل نقاط لهذا النوع من المستخدمين";
        public const string DataValidationErrorArabicMessage = "خطأ في البيانات المدخلة";
        public const string IncorrectUsernameOrPasswordArabicMessage = "خطأ في اسم المستخدم او كلمة السر";
        public const string NegativeNumberArabicMessage = "خطأ قيمة مدخلة اصغر من الصفر";
        public const string DuplicatedDataArabicMessage = "خطأ لا يمكن ادخال معلومات موجودة مسبقا";
        public const string UnValidTokenArabicMessage = "خطأ في ترميز الوصول";
        public const string UnknownErrorArabicMessage = " خطأ غير معروف قد يكون سببه عملية حذف لسجلات مرتبطة مع فقرات أخرى أو خطأ في الإتصال يرجى المحاولة مرة أخرى";
        public const string CantRenewItemHoldArabicMessage = "لا يمكن تجديد فترة حجز المادة";
        public const string QuantityMustBeBiggerThanZeroArabicMessage = "يرجى إدخال كمية أكبر من الصفر";
        public const string HoldPeriodMustBeBiggerThanOneArabicMessage = "يرجى إدخال مدة الحجز أكبر من الواحد";
        public const string OrderHoldPeriodMustBeBiggerThanItemHoldPeriodArabicMessage = "يجب ان تكون مدة حجز الطلبية اكبر من مدة حجز المادة";
        public const string CannotChangeUserFollowPermissionFromTemplateArabicMessage = "لا يمكن تغيير صلاحيات مستخدم مسنده له قالب صلاحيات";
        public const string CannotCancelOrderArabicMessage = "لا يمكن الغاء ترحيل طلبية عند وجود سلة مفتوحة";
        public const string OfferItemShouldHaveInitQuantityArabicMessage = "يجب تحديد كمية المادة بالعرض";
        public const string CannotSubmitEmptyOrderArabicMessage = "لا يمكن ترحيل طلبية ليس لها مواد ";
        public const string CannotUpdateOfferItemArabicMessage = "لا يمكن تعديل مادة عرض عليها حركة ";
        public const string CannotTransferToDisabledUserArabicMessage = "لا يمكن تحويل الى مستخدم غير مفعل  ";
        public const string CannotCancelOrderInThisStatusArabicMessage = "لا يمكن تغيير حالة الطلبية المرحلة  ";
        public const string NoSearchResultArabicMessage = "لا يوجد نتائج بحث  ";
        public const string CannotAddItemToOrderInThisStatusArabicMessage = "لا يمكن الاضافة على السلة في هذه الحالة  ";

        public const string QRCodeIsNotAvailableArabicMessage = "رمز التعريف مستخدم من قبل";
        public const string NotValidAccessTokenArabicMessage = "خطأ في ترميز الوصول";
        public const string UserIsDisabledArabicMessage = "تم ايقاف المستخدم الخاص بكم يرجى مراجعة ادارة النادي";
        public const string UserIsNotActiveArabicMessage = "تم ايقاف تفعيل المستخدم الخاص بكم يرجى مراجعة ادارة النادي";


        public const string FillDataEnglishMessage = "Please fill missing data";
        public const string NoPermissionsCodeEnglishMessage = "You don't have permission to complete this operation";
        public const string UsernameNotAvailableEnglishMessage = "Username is not available";
        public const string MobileNumberIsNotAvailableEnglishMessage = "Mobile number is not available";
        public const string UsernameLengthMustBeEightOrOverCharactersEnglishMessage = "Username length must be 8 characters or more";
        public const string SenderCannotBeReceiverAtTheSameTimeEnglishMessage = "You cannot transfer to your self";
        public const string NoBalanceErrorEnglishMessage = "No Balance";
        public const string CannotTransferToAdminUsersEnglishMessage = "Cannot transfer to admin users";
        public const string NotValidAccessTokenEnglishMessage = "Not valid access token";
        public const string DataValidationErrorEnglishMessage = "Data validation error";
        public const string NegativeNumberEnglishMessage = "Please enter positive value";
        public const string DuplicatedDataEnglishMessage = "Cannot Insert Duplicated Data";
        public const string CantRenewItemHoldEnglishMessage = "Cannot Renew Item Hold ";
        public const string UnValidTokenEnglishMessage = "Token validation error";
        public const string UnknownErrorEnglishMessage = "Unknown Error";
        public const string QuantityMustBeBiggerThanZeroEnglishMessage = "Quantity must be bigger than zero";
        public const string HoldPeriodMustBeBiggerThanOneEnglishMessage = "Hold Period must be bigger than one";
        public const string OrderHoldPeriodMustBeBiggerThanItemHoldPeriodEnglishMessage = "Order Hold Period Must Be Bigger Than Item Hold Period";
        public const string CannotChangeUserFollowPermissionFromTemplateEnglishMessage = "Can't Change User Follow Permission From Template";
        public const string CannotCancelOrderEnglishMessage = "Can't Cancel Order When there is Open Order";
        public const string OfferItemShouldHaveInitQuantityEnglishMessage = "Offer Item Should Have Init Quantity";
        public const string CannotSubmitEmptyOrderEnglishMessage = "Cannot Submit Empty Order";

        public static void LogError(int LoggedUser, string ErrorMessage, string Stack, string Version, string SourcePlatform, string SourceFunction, string Param, string Param2)
        {
            try
            {
                using (SqlConnection conn = ConnectionClass.DataConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Developers_LogError";
                    List<SqlParameter> Params = new List<SqlParameter>()
                    {
                        new SqlParameter("LoggedUser", LoggedUser),
                        new SqlParameter("ErrorMessage", ErrorMessage),
                        new SqlParameter("Stack", Stack),
                        new SqlParameter("Version", Version),
                        new SqlParameter("SourcePlatform", SourcePlatform),
                        new SqlParameter("SourceFunction", SourceFunction),
                        new SqlParameter("Param", Param),
                        new SqlParameter("Param2", Param2),
                    };

                    cmd.Parameters.AddRange(Params.ToArray());
                    cmd.ExecuteReader();
                }
            }
            catch (Exception e)
            {
            }
        }
        public static string GetErrorMessage(int Code)
        {
            switch (Code)
            {
                case FillData: return FillDataArabicMessage;
                case NoPermissions: return NoPermissionsCodeArabicMessage;
                case UsernameNotAvailable: return UsernameNotAvailableArabicMessage;
                case MobileNumberIsNotAvailable: return MobileNumberIsNotAvailableArabicMessage;
                case UsernameLengthMustBeEightOrOverCharacters: return UsernameLengthMustBeEightOrOverCharactersArabicMessage;
                case SenderCannotBeReceiverAtTheSameTime: return SenderCannotBeReceiverAtTheSameTimeArabicMessage;
                case UnknownError: return UnknownErrorArabicMessage;
                case NoBalance: return NoBalanceErrorArabicMessage;
                case QRCodeIsNotAvailable: return QRCodeIsNotAvailableArabicMessage;
                case CannotTransferToAdminUsers: return CannotTransferToAdminUsersArabicMessage;
                case CannotTransferPointsToThisTypeOfUsers: return CannotTransferPointsToThisTypeOfUsersArabicMessage;
                case DataValidationError: return DataValidationErrorArabicMessage;
                case NegativeNumber: return NegativeNumberArabicMessage;
                case CantRenewItemHold: return CantRenewItemHoldArabicMessage;
                case QuantityMustBeBiggerThanZero: return QuantityMustBeBiggerThanZeroArabicMessage;
                case HoldPeriodMustBeBiggerThanOne: return HoldPeriodMustBeBiggerThanOneArabicMessage;
                case OrderHoldPeriodMustBeBiggerThanItemHoldPeriod: return OrderHoldPeriodMustBeBiggerThanItemHoldPeriodArabicMessage;
                case CannotChangeUserFollowPermissionFromTemplate: return CannotChangeUserFollowPermissionFromTemplateArabicMessage;
                case CannotCancelOrder: return CannotCancelOrderArabicMessage;
                case CannotSubmitEmptyOrder: return CannotSubmitEmptyOrderArabicMessage;
                case CannotUpdateOfferItem: return CannotUpdateOfferItemArabicMessage;
                case CannotTransferToDisabledUser: return CannotTransferToDisabledUserArabicMessage;
                case UserIsDisabled: return UserIsDisabledArabicMessage;
                case CannotCancelOrderInThisStatus: return CannotCancelOrderInThisStatusArabicMessage;
                case NoSearchResult: return NoSearchResultArabicMessage;
                case CannotAddItemToOrderInThisStatus: return CannotAddItemToOrderInThisStatusArabicMessage;
                case UserIsNotActive: return UserIsNotActiveArabicMessage;

                case DuplicatedData: return DuplicatedDataArabicMessage;
                case NotValidAccessToken: return NotValidAccessTokenArabicMessage;
                default: return UnknownErrorArabicMessage;
            }
        }
    }
}