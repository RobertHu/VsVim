﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VimCore;
using VimCore.Modes.Command;
using Microsoft.VisualStudio.Text.Editor;
using VimCoreTest.Utils;
using Microsoft.VisualStudio.Text;

namespace VimCoreTest
{
    [TestClass]
    public class CommandModeTest
    {
        private IWpfTextView _view;
        private IVimBufferData _bufferData;
        private CommandMode _modeRaw;
        private IMode _mode;
        private FakeVimHost _host;

        public void Create(params string[] lines)
        {
            _view = Utils.EditorUtil.CreateView(lines);
            _view.Caret.MoveTo(new SnapshotPoint(_view.TextSnapshot, 0));
            _host = new FakeVimHost();
            _bufferData = MockFactory.CreateVimBufferData(
                _view,
                "test",
                _host,
                MockFactory.CreateVimData(new RegisterMap()).Object);
            _modeRaw = new VimCore.Modes.Command.CommandMode(_bufferData);
            _mode = _modeRaw;
            _mode.OnEnter();
        }

        [TestMethod, Description("Entering command mode should update the status")]
        public void StatusOnColon1()
        {
            Create(String.Empty);
            _mode.OnEnter();
            Assert.AreEqual(":", _host.Status);
        }

        [TestMethod, Description("When leaving command mode we should clear the status")]
        public void StatusOnLeave()
        {
            Create(String.Empty);
            _host.Status = "foo";
            _mode.OnLeave();
            Assert.AreEqual(String.Empty, _host.Status);
        }
    }
}