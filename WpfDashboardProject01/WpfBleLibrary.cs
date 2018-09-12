using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfDashboardProject01
{
    public partial class MainWindow : Window
    {
        /* ================================================================ */
        /*                BEGIN MAIN EVENT-DRIVEN APP LOGIC                 */
        /* ================================================================ */

        public const UInt16 STATE_STANDBY = 0;
        public const UInt16 STATE_SCANNING = 1;
        public const UInt16 STATE_CONNECTING = 2;
        public const UInt16 STATE_FINDING_SERVICES = 3;
        public const UInt16 STATE_FINDING_ATTRIBUTES = 4;
        public const UInt16 STATE_LISTENING_MEASUREMENTS = 5;

        public UInt16 app_state = STATE_STANDBY;        // current application state
        //public Byte connection_handle = 0;              // connection handle (will always be 0 if only one connection happens at a time)

        public List<Byte> connection_handle = new List<byte>();              // connection handle (will always be 0 if only one connection happens at a time)

        public UInt16 att_handlesearch_start = 0;       // "start" handle holder during search
        public UInt16 att_handlesearch_end = 0;         // "end" handle holder during search
        public UInt16 att_handle_measurement = 0;       // heart rate measurement attribute handle
        public UInt16 att_handle_measurement_ccc = 0;   // heart rate measurement client characteristic configuration handle (to enable notifications)

        // for master/scanner devices, the "gap_scan_response" event is a common entry-like  
        // this filters ad packets to find devices which advertise the Heart Rate service
        public void GAPScanResponseEvent(object sender, Bluegiga.BLE.Events.GAP.ScanResponseEventArgs e)
        {
            String log = String.Format("ble_evt_gap_scan_response: rssi={0}, packet_type={1}, sender=[ {2}], address_type={3}, bond={4}, data=[ {5}]" + Environment.NewLine,
                (SByte)e.rssi,
                e.packet_type,
                ByteArrayToHexString(e.sender),
                e.address_type,
                e.bond,
                ByteArrayToHexString(e.data)
                );

            //Console.Write(log);
            //ThreadSafeDelegate(delegate { txtLog.AppendText(log); });
            if (debug) SetLogText(log);

            // pull all advertised service info from ad packet
            List<Byte[]> ad_services = new List<Byte[]>();
            Byte[] this_field = { };
            int bytes_left = 0;
            int field_offset = 0;
            for (int i = 0; i < e.data.Length; i++)
            {
                if (bytes_left == 0)
                {
                    bytes_left = e.data[i];
                    this_field = new Byte[e.data[i]];
                    field_offset = i + 1;
                }
                else
                {
                    this_field[i - field_offset] = e.data[i];
                    bytes_left--;
                    if (bytes_left == 0)
                    {
                        if (this_field[0] == 0x02 || this_field[0] == 0x03)
                        {
                            // partial or complete list of 16-bit UUIDs
                            ad_services.Add(this_field.Skip(1).Take(2).Reverse().ToArray());
                        }
                        else if (this_field[0] == 0x04 || this_field[0] == 0x05)
                        {
                            // partial or complete list of 32-bit UUIDs
                            ad_services.Add(this_field.Skip(1).Take(4).Reverse().ToArray());
                        }
                        else if (this_field[0] == 0x06 || this_field[0] == 0x07)
                        {
                            // partial or complete list of 128-bit UUIDs
                            ad_services.Add(this_field.Skip(1).Take(16).Reverse().ToArray());
                        }
                    }
                }
            }

            // check for 0x180D (official heart rate service UUID)
            if (ad_services.Any(a => a.SequenceEqual(new Byte[] { 0x18, 0x0D })))
            {
                // connect to this device
                Byte[] cmd = bglib.BLECommandGAPConnectDirect(e.sender, e.address_type, 0x20, 0x30, 0x100, 0); // 125ms interval, 125ms window, active scanning
                // DEBUG: display bytes written
                //ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("=> TX ({0}) [ {1}]", cmd.Length, ByteArrayToHexString(cmd)) + Environment.NewLine); });
                String logText = String.Format("=> TX ({0}) [ {1}]", cmd.Length, ByteArrayToHexString(cmd)) + Environment.NewLine;
                if (debug) SetLogText(logText);

                bglib.SendCommand(serialAPI, cmd);
                //while (bglib.IsBusy()) ;

                // update state
                app_state = STATE_CONNECTING;
            }
        }

        // the "connection_status" event occurs when a new connection is established
        public void ConnectionStatusEvent(object sender, Bluegiga.BLE.Events.Connection.StatusEventArgs e)
        {
            String log = String.Format("ble_evt_connection_status: connection={0}, flags={1}, address=[ {2}], address_type={3}, conn_interval={4}, timeout={5}, latency={6}, bonding={7}" + Environment.NewLine,
                e.connection,
                e.flags,
                ByteArrayToHexString(e.address),
                e.address_type,
                e.conn_interval,
                e.timeout,
                e.latency,
                e.bonding
                );
            //Console.Write(log);
            //ThreadSafeDelegate(delegate { txtLog.AppendText(log); });
            if (debug) SetLogText(log);

            if ((e.flags & 0x05) == 0x05)
            {
                // connected, now perform service discovery
                //connection_handle = e.connection;

                // Keep the handle in a list here
                connection_handle.Add(e.connection);

                //ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("Connected to {0}", ByteArrayToHexString(e.address)) + Environment.NewLine); });
                String logText = String.Format("Connected to handle: {0} address: {1}", e.connection.ToString(), ByteArrayToHexString(e.address)) + Environment.NewLine;
                SetLogText(logText);

                Byte[] cmd = bglib.BLECommandATTClientReadByGroupType(e.connection, 0x0001, 0xFFFF, new Byte[] { 0x00, 0x28 }); // "service" UUID is 0x2800 (little-endian for UUID uint8array)
                // DEBUG: display bytes written
                //ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("=> TX ({0}) [ {1}]", cmd.Length, ByteArrayToHexString(cmd)) + Environment.NewLine); });
                logText = String.Format("=> TX ({0}) [ {1}]", cmd.Length, ByteArrayToHexString(cmd)) + Environment.NewLine;
                if (debug) SetLogText(logText);

                bglib.SendCommand(serialAPI, cmd);
                //while (bglib.IsBusy()) ;

                // update state
                app_state = STATE_FINDING_SERVICES;
            }
        }

        public void ATTClientGroupFoundEvent(object sender, Bluegiga.BLE.Events.ATTClient.GroupFoundEventArgs e)
        {
            String log = String.Format("ble_evt_attclient_group_found: connection={0}, start={1}, end={2}, uuid=[ {3}]" + Environment.NewLine,
                e.connection,
                e.start,
                e.end,
                ByteArrayToHexString(e.uuid)
                );
            //Console.Write(log);
            //ThreadSafeDelegate(delegate { txtLog.AppendText(log); });
            if (debug) SetLogText(log);

            // found "service" attribute groups (UUID=0x2800), check for heart rate measurement service
            if (e.uuid.SequenceEqual(new Byte[] { 0x0D, 0x18 }))
            {
                //ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("Found attribute group for service w/UUID=0x180D: start={0}, end=%d", e.start, e.end) + Environment.NewLine); });
                String logText = String.Format("Found attribute group for service w/UUID=0x180D: start={0}, end=%d", e.start, e.end) + Environment.NewLine;
                if (debug) SetLogText(logText);

                att_handlesearch_start = e.start;
                att_handlesearch_end = e.end;
            }
        }

        public void ATTClientFindInformationFoundEvent(object sender, Bluegiga.BLE.Events.ATTClient.FindInformationFoundEventArgs e)
        {
            String log = String.Format("ble_evt_attclient_find_information_found: connection={0}, chrhandle={1}, uuid=[ {2}]" + Environment.NewLine,
                e.connection,
                e.chrhandle,
                ByteArrayToHexString(e.uuid)
                );
            //Console.Write(log);
            //ThreadSafeDelegate(delegate { txtLog.AppendText(log); });
            if (debug) SetLogText(log);

            // check for heart rate measurement characteristic (UUID=0x2A37)
            if (e.uuid.SequenceEqual(new Byte[] { 0x37, 0x2A }))
            {
                //ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("Found attribute w/UUID=0x2A37: handle={0}", e.chrhandle) + Environment.NewLine); });
                String logText = String.Format("Found attribute w/UUID=0x2A37: handle={0}", e.chrhandle) + Environment.NewLine;
                if (debug) SetLogText(logText);

                att_handle_measurement = e.chrhandle;
            }
            // check for subsequent client characteristic configuration (UUID=0x2902)
            else if (e.uuid.SequenceEqual(new Byte[] { 0x02, 0x29 }) && att_handle_measurement > 0)
            {
                //ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("Found attribute w/UUID=0x2902: handle={0}", e.chrhandle) + Environment.NewLine); });
                String logText = String.Format("Found attribute w/UUID=0x2902: handle={0}", e.chrhandle) + Environment.NewLine;
                if (debug) SetLogText(logText);

                att_handle_measurement_ccc = e.chrhandle;
            }
        }

        public void ATTClientProcedureCompletedEvent(object sender, Bluegiga.BLE.Events.ATTClient.ProcedureCompletedEventArgs e)
        {
            String log = String.Format("ble_evt_attclient_procedure_completed: connection={0}, result={1}, chrhandle={2}" + Environment.NewLine,
                e.connection,
                e.result,
                e.chrhandle
                );
            //Console.Write(log);
            //ThreadSafeDelegate(delegate { txtLog.AppendText(log); });
            if (debug) SetLogText(log);

            // check if we just finished searching for services
            if (app_state == STATE_FINDING_SERVICES)
            {
                if (att_handlesearch_end > 0)
                {
                    //print "Found 'Heart Rate' service with UUID 0x180D"

                    // found the Heart Rate service, so now search for the attributes inside
                    Byte[] cmd = bglib.BLECommandATTClientFindInformation(e.connection, att_handlesearch_start, att_handlesearch_end);
                    // DEBUG: display bytes written
                    //ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("=> TX ({0}) [ {1}]", cmd.Length, ByteArrayToHexString(cmd)) + Environment.NewLine); });
                    String logText = String.Format("=> TX ({0}) [ {1}]", cmd.Length, ByteArrayToHexString(cmd)) + Environment.NewLine;
                    if (debug) SetLogText(logText);

                    bglib.SendCommand(serialAPI, cmd);
                    //while (bglib.IsBusy()) ;

                    // update state
                    app_state = STATE_FINDING_ATTRIBUTES;
                }
                else
                {
                    //ThreadSafeDelegate(delegate { txtLog.AppendText("Could not find 'Heart Rate' service with UUID 0x180D" + Environment.NewLine); });
                    String logText = "Could not find 'Heart Rate' service with UUID 0x180D" + Environment.NewLine;
                    if (debug) SetLogText(logText);
                }
            }
            // check if we just finished searching for attributes within the heart rate service
            else if (app_state == STATE_FINDING_ATTRIBUTES)
            {
                if (att_handle_measurement_ccc > 0)
                {
                    //print "Found 'Heart Rate' measurement attribute with UUID 0x2A37"

                    // found the measurement + client characteristic configuration, so enable notifications
                    // (this is done by writing 0x0001 to the client characteristic configuration attribute)
                    Byte[] cmd = bglib.BLECommandATTClientAttributeWrite(e.connection, att_handle_measurement_ccc, new Byte[] { 0x01, 0x00 });
                    // DEBUG: display bytes written
                    //ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("=> TX ({0}) [ {1}]", cmd.Length, ByteArrayToHexString(cmd)) + Environment.NewLine); });
                    String logText = String.Format("=> TX ({0}) [ {1}]", cmd.Length, ByteArrayToHexString(cmd)) + Environment.NewLine;
                    if (debug) SetLogText(logText);

                    bglib.SendCommand(serialAPI, cmd);
                    //while (bglib.IsBusy()) ;

                    // update state
                    app_state = STATE_LISTENING_MEASUREMENTS;
                }
                else
                {
                    //ThreadSafeDelegate(delegate { txtLog.AppendText("Could not find 'Heart Rate' measurement attribute with UUID 0x2A37" + Environment.NewLine); });
                    String logText = "Could not find 'Heart Rate' measurement attribute with UUID 0x2A37" + Environment.NewLine;
                    if (debug) SetLogText(logText);
                }
            }
        }

        public void ATTClientAttributeValueEvent(object sender, Bluegiga.BLE.Events.ATTClient.AttributeValueEventArgs e)
        {
            String log = String.Format("ble_evt_attclient_attribute_value: connection={0}, atthandle={1}, type={2}, value=[ {3}]" + Environment.NewLine,
                e.connection,
                e.atthandle,
                e.type,
                ByteArrayToHexString(e.value)
                );
            //Console.Write(log);
            //ThreadSafeDelegate(delegate { txtLog.AppendText(log); });
            if (debug) SetLogText(log);

            /*
            // check for a new value from the connected peripheral's heart rate measurement attribute
            if (e.connection == connection_handle && e.atthandle == att_handle_measurement)
            {
                Byte hr_flags = e.value[0];
                int hr_measurement = e.value[1];
                // #################################################################################################
                // display actual measurement
                //ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("Heart rate: {0} bpm", hr_measurement) + Environment.NewLine); });
                // #################################################################################################
                String logText = String.Format("Heart rate: {0} bpm", hr_measurement) + Environment.NewLine;
                SetLogText(logText);

                heartRate = hr_measurement;

                // Set the LiveCharts Gauge control value;
                SetGaugeValue(heartRate);

                // Add the value to the LiveCharts lineSeries0 Values
                if(myLineSeries0.Values.Count < N)
                {
                    myLineSeries0.Values.Add(heartRate);
                }
                else
                {
                    myLineSeries0.Values.RemoveAt(0);
                    myLineSeries0.Values.Add(heartRate);
                }
                
                //Value = heartRate;
            }
            */

            // Which handle was this?
            foreach (var ahandle in connection_handle)
            {
                if (e.connection == ahandle && e.atthandle == att_handle_measurement)
                {
                    Byte hr_flags = e.value[0];
                    int hr_measurement = e.value[1];
                    // #################################################################################################
                    // display actual measurement
                    //ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("Heart rate: {0} bpm", hr_measurement) + Environment.NewLine); });
                    // #################################################################################################

                    //String logText = String.Format("{0} Handle: {1} Rate: {2} bpm", stopwatch.Elapsed.ToString(), ahandle, hr_measurement) + Environment.NewLine;
                    // For Unix timestamp: https://stackoverflow.com/questions/17632584/how-to-get-the-unix-timestamp-in-c-sharp
                    Int32 timeStamp = new TimeStamp().UnixTimeStampUTC();                    

                    // For Milliseconds timestamp: https://stackoverflow.com/questions/16032451/get-datetime-now-with-milliseconds-precision
                    //string timeStamp = DateTime.UtcNow.ToString("MM-dd-yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);

                    String logText = String.Format("{0} Handle: {1} Rate: {2} bpm", timeStamp, ahandle, hr_measurement) + Environment.NewLine;
                    SetLogText(logText);

                    heartRate = hr_measurement;

                    // Set the LiveCharts Gauge control value;
                    SetGaugeValue(heartRate);

                    // Add the value to the LiveCharts lineSeries0 Values
                    if (myLineSeries0.Values.Count < N)
                    {
                        myLineSeries0.Values.Add(heartRate);
                    }
                    else
                    {
                        myLineSeries0.Values.RemoveAt(0);
                        myLineSeries0.Values.Add(heartRate);
                    }

                    //Value = heartRate;
                }
            }
            
        }

        /* ================================================================ */
            /*                 END MAIN EVENT-DRIVEN APP LOGIC                  */
            /* ================================================================ */


            // Thread-safe operations from event handlers
            // I love StackOverflow: http://stackoverflow.com/q/782274
            /*
            public void ThreadSafeDelegate(MethodInvoker method)
            {
                if (InvokeRequired)
                    BeginInvoke(method);
                else
                    method.Invoke();
            }
            */

            // Convert byte array to "00 11 22 33 44 55 " string
            public string ByteArrayToHexString(Byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2} ", b);
            return hex.ToString();
        }

        // Serial port event handler for a nice event-driven architecture
        private void DataReceivedHandler(
                                object sender,
                                System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            System.IO.Ports.SerialPort sp = (System.IO.Ports.SerialPort)sender;
            Byte[] inData = new Byte[sp.BytesToRead];

            // read all available bytes from serial port in one chunk
            sp.Read(inData, 0, sp.BytesToRead);

            // DEBUG: display bytes read
            //ThreadSafeDelegate(delegate { txtLog.AppendText(String.Format("<= RX ({0}) [ {1}]", inData.Length, ByteArrayToHexString(inData)) + Environment.NewLine); });
            String logText = String.Format("<= RX ({0}) [ {1}]", inData.Length, ByteArrayToHexString(inData)) + Environment.NewLine;
            if (debug) SetLogText(logText);

            // parse all bytes read through BGLib parser
            for (int i = 0; i < inData.Length; i++)
            {
                bglib.Parse(inData[i]);
            }
        }

        public class TimeStamp
        {
            public Int32 UnixTimeStampUTC()
            {
                Int32 unixTimeStamp;
                DateTime currentTime = DateTime.Now;
                DateTime zuluTime = currentTime.ToUniversalTime();
                DateTime unixEpoch = new DateTime(1970, 1, 1);
                unixTimeStamp = (Int32)(zuluTime.Subtract(unixEpoch)).TotalSeconds;
                return unixTimeStamp;
            }
        }
    }
}
