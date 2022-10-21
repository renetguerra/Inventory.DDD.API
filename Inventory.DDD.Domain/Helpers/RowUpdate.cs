using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.DDD.Domain.Helpers
{
    /// <summary>
    /// Guarda en bbdd la informcion correspondiente a los campos de la clase RowIACDU
    /// para el control y seguimiento de las trazas de datos.
    /// </summary>
    public class RowUpdate
    {
        // Enums
        public enum UpdateType { Insert = 1000, ReActivate = 2000, Update = 3000, DeActivate = 4000, Delete = 5000 }

        public enum UpdateCode
        {
            Process_DB_Initialize = 1,

            ProcessByUser = 100,
            Process_ImportXL = 101,


            Process_SP = 200,
            Process_SP_OCR = 201,

            Process_Generic = 300,
            Process_NewOperator = 301,
            Process_Migration = 302,
            Process_Buy = 311,
            Process_Reminder = 312,

            Process_WebService = 400,

            Process_Login = 501,
            Process_ResendEmail = 502
        }

        // Public Properties ---------------------------------------------------------------------------------

        public const int User_DB = -1;
        public const int User_SP = -2;
        public const int User_Process = -3;
        public const int User_WebService = -4;
        public const int User_Test = -5;
        public const int User_WindowsService = -6;

        public RowIACDU RowModel { get; set; }
        public RowUpdate(RowIACDU model)
        {
            this.RowModel = model;
        }

        public void RowSave(int userId, UpdateType type) => RowSave(userId, true, type, UpdateCode.ProcessByUser);

        public void RowSave(int userId, bool active, UpdateType type, UpdateCode code)
        {
            if (type == 0)
                type = this.RowModel.Id.Equals(0) ? UpdateType.Insert : UpdateType.Update;

            if (code == 0)
                code = UpdateCode.ProcessByUser;

            if (type == UpdateType.DeActivate || type == UpdateType.Delete)
                active = false;

            var updateCode = (int)type + (int)code;

            RowSetModel(active, updateCode, userId);
        }

        private void RowSetModel(bool active, int code, int userId)
        {
            this.RowModel.RowActive = active;
            this.RowModel.RowUpdateCode = code;
            this.RowModel.RowUpdateDate = DateTime.Now;
            this.RowModel.RowUpdateUser = userId;
        }
    }
}
