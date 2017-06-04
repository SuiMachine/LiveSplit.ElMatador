using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveSplit.ComponentUtil;

namespace LiveSplit.ElMatador
{
    class GameMemory
    {
        public bool isSteam;

        public event EventHandler OnLoadStarted;
        public event EventHandler OnLoadFinished;

        private Task _thread;
        private CancellationTokenSource _cancelSource;
        private SynchronizationContext _uiThread;
        private List<int> _ignorePIDs;
        private ElMatadorSettings _settings;

        private DeepPointer _isLoadingPtr;
        private bool hasDLLInjected = false;

        /*
        private enum ExpectedDllSizes
        {
            Steam = 6930432
        }*/

        public bool[] splitStates { get; set; }

        public GameMemory(ElMatadorSettings componentSettings)
        {
            _settings = componentSettings;

            _isLoadingPtr = new DeepPointer("ElMatadorGraphWrapper.dll", 0x3030);  // == 1 if a loadscreen is happening
            _ignorePIDs = new List<int>();
        }

        public void StartMonitoring()
        {
            if (_thread != null && _thread.Status == TaskStatus.Running)
            {
                throw new InvalidOperationException();
            }
            if (!(SynchronizationContext.Current is WindowsFormsSynchronizationContext))
            {
                throw new InvalidOperationException("SynchronizationContext.Current is not a UI thread.");
            }

            _uiThread = SynchronizationContext.Current;
            _cancelSource = new CancellationTokenSource();
            _thread = Task.Factory.StartNew(MemoryReadThread);
        }

        public void Stop()
        {
            if (_cancelSource == null || _thread == null || _thread.Status != TaskStatus.Running)
            {
                return;
            }

            _cancelSource.Cancel();
            _thread.Wait();
        }

        bool isLoading = false;
        bool prevIsLoading = false;
        bool loadingStarted = false;

        void MemoryReadThread()
        {
            Debug.WriteLine("[NoLoads] MemoryReadThread");

            while (!_cancelSource.IsCancellationRequested)
            {
                try
                {
                    Debug.WriteLine("[NoLoads] Waiting for pc_matador.exe...");

                    Process game;
                    while ((game = GetGameProcess()) == null)
                    {
                        Thread.Sleep(250);
                        if (_cancelSource.IsCancellationRequested)
                        {
                            return;
                        }
                    }

                    Debug.WriteLine("[NoLoads] Got games process!");

                    uint frameCounter = 0;

                    while (!game.HasExited && hasDLLInjected)
                    {
                        _isLoadingPtr.Deref(game, out isLoading);

                        if (isLoading != prevIsLoading)
                        {
                            if (isLoading)
                            {
                                Debug.WriteLine(String.Format("[NoLoads] Load Start - {0}", frameCounter));

                                loadingStarted = true;

                                // pause game timer
                                _uiThread.Post(d =>
                                {
                                    if (this.OnLoadStarted != null)
                                    {
                                        this.OnLoadStarted(this, EventArgs.Empty);
                                    }
                                }, null);
                            }
                            else
                            {
                                Debug.WriteLine(String.Format("[NoLoads] Load End - {0}", frameCounter));

                                if (loadingStarted)
                                {
                                    loadingStarted = false;

                                    // unpause game timer
                                    _uiThread.Post(d =>
                                    {
                                        if (this.OnLoadFinished != null)
                                        {
                                            this.OnLoadFinished(this, EventArgs.Empty);
                                        }
                                    }, null);
                                }
                            }
                        }

                        prevIsLoading = isLoading;
                        frameCounter++;

                        Thread.Sleep(15);

                        if (_cancelSource.IsCancellationRequested)
                        {
                            return;
                        }
                    }

                    // pause game timer on exit or crash
                    _uiThread.Post(d =>
                    {
                        if (this.OnLoadStarted != null)
                        {
                            this.OnLoadStarted(this, EventArgs.Empty);
                        }
                    }, null);
                    isLoading = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    Thread.Sleep(1000);
                }
            }
        }


        Process GetGameProcess()
        {
            Process game = Process.GetProcesses().FirstOrDefault(p => (p.ProcessName.ToLower() == "pc_matador") && !p.HasExited && !_ignorePIDs.Contains(p.Id));
            if (game == null)
            {
                hasDLLInjected = false;
                return null;
            }

            if (!_ignorePIDs.Contains(game.Id) && game.ModulesWow64Safe().Any(mod => mod.ModuleName.ToLower() != "elmatadorgraphwrapper.dll"))
            {
                Thread.Sleep(500);
                bool injectedNow = game.ModulesWow64Safe().Any(mod => mod.ModuleName.ToLower() != "elmatadorgraphwrapper.dll");
                if (!injectedNow)
                {
                    _ignorePIDs.Add(game.Id);
                    _uiThread.Send(d => MessageBox.Show("No wrapper detected in the memory. Please install the wrapper and restart the game.", "LiveSplit.ElMatador",
                        MessageBoxButtons.OK, MessageBoxIcon.Error), null);
                    hasDLLInjected = false;
                    return null;
                }
                else
                {
                    hasDLLInjected = true;
                    return game;
                }
            }
            else
                hasDLLInjected = true;


            return game;
        }
    }
}
