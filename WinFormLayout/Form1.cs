using Newtonsoft.Json;
using WinFormLayout.Object;

namespace WinFormLayout
{
    public partial class Form1 : Form
    {
        private Size formOriginalSize;
        private List<PanelRecObject> panelRec;
        private AppSettingsObject _appSettingsObject;
        private int IdConfLayout;
        public Form1()
        {
            panelRec = new List<PanelRecObject>();
            _appSettingsObject = GetSettings();
            IdConfLayout = _appSettingsObject.IdConfLayout;
            InitializeComponent();
            formOriginalSize = this.Size;
            var data = new List<ContentObject>();
            data.Add(new ContentObject()
            {
                Name = "Test",
                Type = ContentType.Image
            });
            data.Add(new ContentObject()
            {
                Name = "Test2",
                Type = ContentType.Image
            });
            data.Add(new ContentObject()
            {
                Name = "Test3",
                Type = ContentType.Image
            });
            data.Add(new ContentObject()
            {
                Name = "Test4",
                Type = ContentType.Image
            });

            SetContent(data);

            this.Resize += form_content_resize;
        }

        private void form_content_resize(object sender, EventArgs e) {
            foreach (var item in panelRec)
            {
                resize_Control(item.panel, item.rec);
            }
        }

        private void resize_Control(Control c, Rectangle r)
        {
            float xRatio = (float)(this.Width) / (float)(formOriginalSize.Width);
            float yRatio = (float)(this.Height) / (float)(formOriginalSize.Height);
            int newX = (int)(r.X * xRatio);
            int newY = (int)(r.Y * yRatio);

            int newWidth = (int)(r.Width * xRatio);
            int newHeight = (int)(r.Height * yRatio);

            c.Location = new Point(newX, newY);
            c.Size = new Size(newWidth, newHeight);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void SetContent(List<ContentObject> data)
        {
            foreach (var item in data.Select((value, i) => new { i, value }))
            {
                var dataConf = _appSettingsObject.ConfLocSize.Where(d => d.id == IdConfLayout).FirstOrDefault().layoutConf[item.i];
                var panel = new Panel();
                this.Controls.Add(panel);
                panel.Name = item.value.Name;
                panel.BackColor = Color.Red;
                panel.Location = new Point(dataConf.Location.x, dataConf.Location.y);
                panel.Name = "Panel" + (item.i + 1);
                panel.Size = new Size(dataConf.Size.width, dataConf.Size.height);
                panel.TabIndex = 0;
                SuspendLayout();

                panelRec.Add(new PanelRecObject()
                {
                    panel = panel,
                    rec = new Rectangle(panel.Location, panel.Size)
                });
            }
        }

        private AppSettingsObject GetSettings()
        {
            var jsonText = File.ReadAllText("appSettings.json");
            return JsonConvert.DeserializeObject<AppSettingsObject>(jsonText);
        }
    }
}