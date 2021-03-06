using System;
using System.Linq;
using Eto.Forms;
using GW2Scratch.EVTCAnalytics.Events;

namespace GW2Scratch.EVTCInspector
{
	public class EventContentFilterControl : Panel
	{
		private uint BuffId { get; set; }
		private bool BuffIdEnabled { get; set; }
		private uint BuffDamageId { get; set; }
		private bool BuffDamageIdEnabled { get; set; }

		private readonly Control buffLayout;
		private readonly Control buffDamageLayout;

		public EventContentFilterControl()
		{
			buffLayout = ConstructBuffLayout();
			buffDamageLayout = ConstructBuffDamageLayout();
			var layout = new DynamicLayout();
			layout.BeginVertical();
			{
				layout.Add(buffLayout);
				layout.Add(buffDamageLayout);
			}
			layout.EndVertical();
			Content = layout;
		}

		public void UpdateContent(EventListControl eventList)
		{
			var selectedTypes = eventList.TypeFilters.Where(x => x.Checked.HasValue && x.Checked.Value).ToArray();
			if (selectedTypes.Length == 0)
			{
				return;
			}

			var commonAncestor = selectedTypes.First().Type;
			foreach (var type in selectedTypes)
			{
				commonAncestor = GetClosestType(commonAncestor, type.Type);
			}

			buffLayout.Visible = typeof(BuffEvent).IsAssignableFrom(commonAncestor);
			buffDamageLayout.Visible = typeof(BuffDamageEvent).IsAssignableFrom(commonAncestor);

			/*
			else if (typeof(AgentEvent).IsAssignableFrom(commonAncestor))
			{
				var filterAgentRadioButton = new RadioButton {Text = "Exact Agent"};
				var filterAgentDataRadioButton = new RadioButton(filterAgentRadioButton) {Text = "Agent data"};
				var agentsDropDown = new DropDown
				{
					DataStore = agents,
					ItemTextBinding = new DelegateBinding<Agent, string>(x => $"{x.Name} ({x.Address})")
				};

				var filterLayout = new DynamicLayout();
				filterLayout.BeginHorizontal();
				filterLayout.BeginGroup("Agent filter");
				filterLayout.BeginVertical(spacing: new Size(5, 5));
				filterLayout.AddRow(filterAgentRadioButton, null, agentsDropDown);
				filterLayout.AddRow(filterAgentDataRadioButton, new CheckBox {Text = "Name"}, new TextBox {Text = ""});
				filterLayout.AddRow(null, new CheckBox {Text = "Type"},
					new DropDown {DataStore = new[] {"Player", "NPC", "Gadget"}});
				filterLayout.AddRow(null);
				filterLayout.EndVertical();
				filterLayout.EndGroup();
				filterLayout.EndHorizontal();
				filterPanel.Content = filterLayout;
			}
			*/
		}

		private Control ConstructBuffLayout()
		{
			var buffIdTextBox = new NumericMaskedTextBox<uint> {Value = 0};
			buffIdTextBox.ValueBinding.Bind(() => BuffId, x => BuffId = x);

			var buffIdCheckbox = new CheckBox {Checked = false};
			buffIdCheckbox.CheckedBinding.Bind(() => BuffIdEnabled, x => BuffIdEnabled = x ?? false);

			var filterLayout = new DynamicLayout();
			filterLayout.BeginHorizontal();
			{
				filterLayout.BeginGroup("Buff");
				{
					filterLayout.AddRow("Buff ID", buffIdTextBox, buffIdCheckbox, null);
				}
				filterLayout.EndGroup();
				filterLayout.AddRow(null);
			}
			filterLayout.EndHorizontal();
			return filterLayout;
		}

		private Control ConstructBuffDamageLayout()
		{
			var buffIdTextBox = new NumericMaskedTextBox<uint> {Value = 0};
			buffIdTextBox.ValueBinding.Bind(() => BuffDamageId, x => BuffDamageId = x);

			var buffIdCheckbox = new CheckBox {Checked = false};
			buffIdCheckbox.CheckedBinding.Bind(() => BuffDamageIdEnabled, x => BuffDamageIdEnabled = x ?? false);

			var filterLayout = new DynamicLayout();
			filterLayout.BeginHorizontal();
			{
				filterLayout.BeginGroup("Buff damage");
				{
					filterLayout.AddRow("Buff ID", buffIdTextBox, buffIdCheckbox, null);
				}
				filterLayout.EndGroup();
				filterLayout.AddRow(null);
			}
			filterLayout.EndHorizontal();
			return filterLayout;
		}

		public bool FilterEvent(Event e)
		{
			if (e is BuffEvent buffEvent)
			{
				if (BuffIdEnabled)
				{
					return buffEvent.Buff.Id == BuffId;
				}
			}

			if (e is BuffDamageEvent buffDamage)
			{
				if (BuffDamageIdEnabled)
				{
					return buffDamage.Skill.Id == BuffDamageId;
				}
			}

			return true;
		}

		private Type GetClosestType(Type a, Type b)
		{
			while (a != null)
			{
				if (a.IsAssignableFrom(b))
					return a;

				a = a.BaseType;
			}

			return null;
		}
	}
}