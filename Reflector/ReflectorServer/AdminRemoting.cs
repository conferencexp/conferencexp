using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;


namespace MSR.LST.ConferenceXP.ReflectorService
{
    public class AdminRemoting : MarshalByRefObject
    {
        private readonly RegistrarServer registrar;

        public AdminRemoting()
        {
            ReflectorMgr mgr = ReflectorMgr.getInstance();
            this.registrar = mgr.RegServer;
        }

        public void ForceLeave(ClientEntry entry)
        {
            registrar.ForceLeave(entry);
        }

        public ClientEntry[] GetClientTable()
        {
            ClientEntry[] clientTable = null;

            lock (registrar.ClientRegTable)
            {
                int count = registrar.ClientRegTable.Count;
                if (count > 0)
                {
                    clientTable = new ClientEntry[count];
                    registrar.ClientRegTable.Values.CopyTo(clientTable, 0);
                }
            }

            return clientTable;
        }
    }
}