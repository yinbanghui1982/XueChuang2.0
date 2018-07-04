using System.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;


namespace ScienceResearchWpfApplication
{
    /// <summary>
    /// KeywordTreeUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class KeywordTreeUserControl : UserControl
    {
        ScienceResearchDataSetNew.关键词DataTable gjc_dt;

        ScienceResearchDataSetNewTableAdapters.关键词TableAdapter gjc_ta;
        ScienceResearchDataSetNew.单词DataTable dc_dt;
        ScienceResearchDataSetNewTableAdapters.单词TableAdapter dc_ta;
        ScienceResearchDataSetNew.单词_关键词DataTable dc_gjc_dt;
        ScienceResearchDataSetNewTableAdapters.单词_关键词TableAdapter dc_gjc_ta;

        ScienceResearchDataSetNew.短语DataTable dy_dt;
        ScienceResearchDataSetNewTableAdapters.短语TableAdapter dy_ta;
        ScienceResearchDataSetNew.短语_关键词DataTable dy_gjc_dt;
        ScienceResearchDataSetNewTableAdapters.短语_关键词TableAdapter dy_gjc_ta;

        ObservableCollection<Node> roots = new ObservableCollection<Node>();
        ObservableCollection<Node> node_list = new ObservableCollection<Node>();

        /// <summary>
        /// 当前关键词及其所有子关键词
        /// </summary>
        public List<ScienceResearchDataSetNew.关键词Row> currentGjcAndSons=new List<ScienceResearchDataSetNew.关键词Row>();

        int newDataId;
        Node node_jianqie;

        /// <summary>
        /// 构造函数
        /// </summary>
        public KeywordTreeUserControl()
        {
            InitializeComponent();
            gjc_dt = MainWindow.gjc_dt;
            gjc_ta = MainWindow.gjc_ta;
            gjc_ta.Adapter.RowUpdated += Adapter_RowUpdated;

            dc_dt = MainWindow.dc_dt;
            dc_ta = MainWindow.dc_ta;
            dc_ta.Adapter.RowUpdated += Adapter_RowUpdated;
            dc_gjc_dt = MainWindow.dc_gjc_dt;
            dc_gjc_ta = MainWindow.dc_gjc_ta;

            dy_dt = MainWindow.dy_dt;
            dy_ta = MainWindow.dy_ta;
            dy_ta.Adapter.RowUpdated += Adapter_RowUpdated;
            dy_gjc_dt = MainWindow.dy_gjc_dt;
            dy_gjc_ta = MainWindow.dy_gjc_ta;

            creat_tree();
            linkComboBox.SelectedItem = lblKeywordMapping;            
        }

        private void Adapter_RowUpdated(object sender, OleDbRowUpdatedEventArgs e)
        {
            if ((e.Status == UpdateStatus.Continue) && e.StatementType == StatementType.Insert)
            {
                int newID = 0;
                OleDbCommand cmdGetId = new OleDbCommand("SELECT @@IDENTITY", e.Command.Connection);
                newID = (int)cmdGetId.ExecuteScalar();
                newDataId = newID;
                if (newID == 0)
                {
                    MessageBox.Show("获取ID值错误！");
                }
            }
        }

        private void creat_tree()
        {
            node_list.Clear();
            roots.Clear();

            for (int i = 0; i < gjc_dt.Rows.Count; i++)
            {
                Node node = new Node();
                node.Data = gjc_dt[i];
                node_list.Add(node);
            }
            //----------------建立关系----------------------
            for (int i = 0; i < node_list.Count; i++)
            {
                Node node = node_list[i];
                Node fatherNode = get_father(node);
                if (fatherNode == null)
                {
                    roots.Add(node_list[i]);
                }
                else
                {
                    fatherNode.Children.Add(node);
                }

            }

            //----------------Children排序----------------------


            //roots.Sort(delegate (Node x, Node y)
            //{
            //    return x.Data.关键词.CompareTo(y.Data.关键词);
            //});
            //for (int i = 0; i < node_list.Count; i++)
            //{
            //    Node node = node_list[i];
            //    node.Children.Sort(delegate (Node x, Node y)
            //    {
            //        return x.Data.关键词.CompareTo(y.Data.关键词);
            //    });
            //}
            roots = new ObservableCollection<Node>(roots.OrderBy(item => item.Data.关键词));
            for (int i = 0; i < node_list.Count; i++)
            {
                Node node = node_list[i];
                node.Children = new ObservableCollection<Node>(node.Children.OrderBy(item => item.Data.关键词));
            }

            //----------------设置树视图源----------------------
            keywordTreeView.ItemsSource = roots;
        }


        private Node get_father(Node node)
        {
            int fatherId = node.Data.父节点;
            if (fatherId == 0)
            {                
                return null;
            }
            else
            {
                var father_list = (from nd in node_list
                                   where nd.Data.ID == fatherId
                                   select nd).ToList();                
                return father_list[0];
            }
        }

        /// <summary>
        /// 获取当前节点的所有子节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private List<Node> get_sons(Node node)
        {
            int nodeId = node.Data.ID;
            var father_list = (from nd in node_list
                                where nd.Data.父节点 == nodeId
                                select nd).ToList();
            return father_list;
        }

        private void  get_currentGjcAndSons(Node node)
        {
            List<Node> sons_list = get_sons(node);
            foreach (Node son in sons_list)
            {
                currentGjcAndSons.Add(son.Data);
                get_currentGjcAndSons(son);
            }
        }

        private void keywordTreeView_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = e.OriginalSource as TreeViewItem;
            item.IsExpanded = true;

            Node currentNode = (Node)item.DataContext;
            currentGjcAndSons.Clear();
            currentGjcAndSons.Add(currentNode.Data);
            get_currentGjcAndSons(currentNode);

            MainWindow.mainWindow.statusBar.Items.Clear();
            TextBlock txtb = new TextBlock();
            txtb.Text = "选中的关键词为：ID=" + ((Node)item.DataContext).Data.ID + "，关键词=" + ((Node)item.DataContext).Data.关键词;
            MainWindow.mainWindow.statusBar.Items.Add(txtb);

            if (linkComboBox.SelectedItem == lblKeywordMapping)
            {
                MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.keywordMappingTabItem;
                int id = ((Node)item.DataContext).Data.ID;
                MainWindow.keywordMappingUserControl.keywordDataGrid.SelectedValuePath = "ID";
                MainWindow.keywordMappingUserControl.keywordDataGrid.SelectedValue = id;
                MainWindow.keywordMappingUserControl.keywordDataGrid.ScrollIntoView(MainWindow.keywordMappingUserControl.keywordDataGrid.SelectedItem);
            }
            else if (linkComboBox.SelectedItem == lblNone)
            {
            }
            else if (linkComboBox.SelectedItem == lblKeyword)
            {
                MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.dataBaseTabItem_gjc;
                int id = ((Node)item.DataContext).Data.ID;
                MainWindow.dataBaseUserControl_gjc.contentDataGrid.SelectedValuePath = "ID";
                MainWindow.dataBaseUserControl_gjc.contentDataGrid.SelectedValue = id;
                MainWindow.dataBaseUserControl_gjc.contentDataGrid.ScrollIntoView(MainWindow.dataBaseUserControl_gjc.contentDataGrid.SelectedItem);
            }
            else if (linkComboBox.SelectedItem == lblDc)
            {
                MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.dataBaseTabItem_dc;
                int id = ((Node)item.DataContext).Data.ID;
                MainWindow.dataBaseUserControl_dc.keywordAllDataGrid.SelectedValuePath = "ID";
                MainWindow.dataBaseUserControl_dc.keywordAllDataGrid.SelectedValue = id;
                MainWindow.dataBaseUserControl_dc.keywordAllDataGrid.ScrollIntoView(MainWindow.dataBaseUserControl_dc.keywordAllDataGrid.SelectedItem);
            }
            else if (linkComboBox.SelectedItem == lblDc2)
            {
                MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.dataBaseTabItem_dc;

                string word = ((Node)item.DataContext).Data.关键词;
                MainWindow.dataBaseUserControl_dc.contentDataGrid.SelectedValuePath = "单词";
                MainWindow.dataBaseUserControl_dc.contentDataGrid.SelectedValue = word;
                if (MainWindow.dataBaseUserControl_dc.contentDataGrid.SelectedItem != null)
                    MainWindow.dataBaseUserControl_dc.contentDataGrid.ScrollIntoView(MainWindow.dataBaseUserControl_dc.contentDataGrid.SelectedItem);
                else
                    MessageBox.Show("不存在单词：" + word);
            }
            else if (linkComboBox.SelectedItem == lblDy)
            {
                MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.dataBaseTabItem_dy;
                int id = ((Node)item.DataContext).Data.ID;
                MainWindow.dataBaseUserControl_dy.keywordAllDataGrid.SelectedValuePath = "ID";
                MainWindow.dataBaseUserControl_dy.keywordAllDataGrid.SelectedValue = id;
                MainWindow.dataBaseUserControl_dy.keywordAllDataGrid.ScrollIntoView(MainWindow.dataBaseUserControl_dy.keywordAllDataGrid.SelectedItem);
            }
            else if (linkComboBox.SelectedItem == lblJx)
            {
                MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.dataBaseTabItem_jx;
                int id = ((Node)item.DataContext).Data.ID;
                MainWindow.dataBaseUserControl_jx.keywordAllDataGrid.SelectedValuePath = "ID";
                MainWindow.dataBaseUserControl_jx.keywordAllDataGrid.SelectedValue = id;
                MainWindow.dataBaseUserControl_jx.keywordAllDataGrid.ScrollIntoView(MainWindow.dataBaseUserControl_jx.keywordAllDataGrid.SelectedItem);
            }
            else if (linkComboBox.SelectedItem == lblYd)
            {
                MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.dataBaseTabItem_yd;
                int id = ((Node)item.DataContext).Data.ID;
                MainWindow.dataBaseUserControl_yd.keywordAllDataGrid.SelectedValuePath = "ID";
                MainWindow.dataBaseUserControl_yd.keywordAllDataGrid.SelectedValue = id;
                MainWindow.dataBaseUserControl_yd.keywordAllDataGrid.ScrollIntoView(MainWindow.dataBaseUserControl_yd.keywordAllDataGrid.SelectedItem);
            }
            else if (linkComboBox.SelectedItem == lblWz)
            {
                MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.dataBaseTabItem_wz;
                int id = ((Node)item.DataContext).Data.ID;
                MainWindow.dataBaseUserControl_wz.keywordAllDataGrid.SelectedValuePath = "ID";
                MainWindow.dataBaseUserControl_wz.keywordAllDataGrid.SelectedValue = id;
                MainWindow.dataBaseUserControl_wz.keywordAllDataGrid.ScrollIntoView(MainWindow.dataBaseUserControl_wz.keywordAllDataGrid.SelectedItem);
            }
            else if (linkComboBox.SelectedItem == lblXm)
            {
                MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.dataBaseTabItem_xm;
                int id = ((Node)item.DataContext).Data.ID;
                MainWindow.dataBaseUserControl_xm.keywordAllDataGrid.SelectedValuePath = "ID";
                MainWindow.dataBaseUserControl_xm.keywordAllDataGrid.SelectedValue = id;
                MainWindow.dataBaseUserControl_xm.keywordAllDataGrid.ScrollIntoView(MainWindow.dataBaseUserControl_xm.keywordAllDataGrid.SelectedItem);
            }
            else if (linkComboBox.SelectedItem == lblTp)
            {
                MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.dataBaseTabItem_tp;
                int id = ((Node)item.DataContext).Data.ID;
                MainWindow.dataBaseUserControl_tp.keywordAllDataGrid.SelectedValuePath = "ID";
                MainWindow.dataBaseUserControl_tp.keywordAllDataGrid.SelectedValue = id;
                MainWindow.dataBaseUserControl_tp.keywordAllDataGrid.ScrollIntoView(MainWindow.dataBaseUserControl_tp.keywordAllDataGrid.SelectedItem);
            }
            else if (linkComboBox.SelectedItem == lblTpcz)
            {
                MainWindow.mainWindow.rightTabControl.SelectedItem = MainWindow.dataBaseTabItem_tpcz;
                int id = ((Node)item.DataContext).Data.ID;
                MainWindow.dataBaseUserControl_tpcz.keywordAllDataGrid.SelectedValuePath = "ID";
                MainWindow.dataBaseUserControl_tpcz.keywordAllDataGrid.SelectedValue = id;
                MainWindow.dataBaseUserControl_tpcz.keywordAllDataGrid.ScrollIntoView(MainWindow.dataBaseUserControl_tpcz.keywordAllDataGrid.SelectedItem);
            }
        }

        private void btnHalf_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.half();
        }

        private void btnFull_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.right_zero();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {            
            keywordTreeView.ItemsSource = null;
            creat_tree();
        }

        private void btnExpand_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in keywordTreeView.Items)
            {
                DependencyObject dObject = keywordTreeView.ItemContainerGenerator.ContainerFromItem(item);
                ((TreeViewItem)dObject).ExpandSubtree();
            }
        }

        private void btnZhedie_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in keywordTreeView.Items)
            {
                DependencyObject dObject = keywordTreeView.ItemContainerGenerator.ContainerFromItem(item);
                CollapseTreeviewItems(((TreeViewItem)dObject));
            }
        }

        private void CollapseTreeviewItems(TreeViewItem Item)
        {
            Item.IsExpanded = false;

            foreach (var item in Item.Items)
            {
                DependencyObject dObject = keywordTreeView.ItemContainerGenerator.ContainerFromItem(item);

                if (dObject != null)
                {
                    ((TreeViewItem)dObject).IsExpanded = false;

                    if (((TreeViewItem)dObject).HasItems)
                    {
                        CollapseTreeviewItems(((TreeViewItem)dObject));
                    }
                }
            }
        }

        /// <summary>
        /// 为选择的关键词增加一个子关键词
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddSonKeyword_Click(object sender, RoutedEventArgs e)
        {
            Node node_father = (Node)keywordTreeView.SelectedItem;
            int id_father = 0;
            Node node_child_new = new Node();            

            if (node_father == null)
            {
                MessageBox.Show("请选择一个节点");
            }
            else
            {
                id_father = node_father.Data.ID;

                DataRow newRow;
                newRow = gjc_dt.NewRow();
                newRow["关键词"] = keywordTextBox.Text;
                newRow["父节点"] = id_father;
                gjc_dt.Rows.Add(newRow);

                try
                {
                    gjc_ta.Update(gjc_dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                    //gjc_ta.Fill(gjc_dt);
                    gjc_dt.Rows.Remove(newRow);
                    return;
                }
                
                newRow["ID"] = newDataId;

                ScienceResearchDataSetNew.关键词Row gjc_row =(ScienceResearchDataSetNew.关键词Row)newRow;

                node_child_new.Data = gjc_row;
                node_list.Add(node_child_new);

                node_father.Children.Add(node_child_new);

                Node node = node_father;
                node.Children = new ObservableCollection<Node>(node.Children.OrderBy(item => item.Data.关键词));
            }
        }

        private void btnAddRootKeyword_Click(object sender, RoutedEventArgs e)
        {
            int id_father = 0;
            Node node_child_new = new Node();

            DataRow newRow;
            newRow = gjc_dt.NewRow();
            newRow["关键词"] = keywordTextBox.Text;
            newRow["父节点"] = id_father;
            gjc_dt.Rows.Add(newRow);
            try
            {
                gjc_ta.Update(gjc_dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                gjc_dt.Rows.Remove(newRow);
                return;
            }
            newRow["ID"] = newDataId;

            ScienceResearchDataSetNew.关键词Row gjc_row = (ScienceResearchDataSetNew.关键词Row)newRow;
            node_child_new.Data = gjc_row;
            node_list.Add(node_child_new);
            roots.Add(node_child_new);
            //roots = new ObservableCollection<Node>(roots.OrderBy(item => item.Data.关键词));
            creat_tree();

        }

        /// <summary>
        /// 修改关键词
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditKeyword_Click(object sender, RoutedEventArgs e)
        {
            Node node = (Node)keywordTreeView.SelectedItem;
            if (node.Data.父节点 != 0)
            {
                string gjc2 = node.Data.关键词;
                node.Data.关键词 = keywordTextBox.Text;
                try
                {
                    gjc_ta.Update(gjc_dt);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    node.Data.关键词 = gjc2;
                    return;
                }
                
                get_father(node).Children = new ObservableCollection<Node>(get_father(node).Children.OrderBy(item => item.Data.关键词));
            }
            else
            {
                string gjc2 = node.Data.关键词;
                node.Data.关键词 = keywordTextBox.Text;

                try
                {
                    gjc_ta.Update(gjc_dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    node.Data.关键词 = gjc2;
                    return;
                }
                //roots = new ObservableCollection<Node>(roots.OrderBy(item => item.Data.关键词));
                creat_tree();
            }

        }

        private void btnDeleteKeyword_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确认删除该关键词及其所有子关键词", "重要", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                //
            }
            else
            {
                return;
            }
            Node node = (Node)keywordTreeView.SelectedItem;
            deleteKeyword(node);     

        }

        private void deleteKeyword(Node node)
        {         
            if (node.Children.Count != 0)
            {
                for (int i = node.Children.Count - 1; i >= 0; i--)
                {
                    deleteKeyword(node.Children[i]);
                }
                deleteKeyword(node);
            }
            else
            {
                Node node_father = get_father(node);
                if (node_father != null)
                {
                    node_father.Children.Remove(node);
                    node_list.Remove(node);
                    node.Data.Delete();
                    try
                    {
                        gjc_ta.Update(gjc_dt);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    roots.Remove(node);
                    node_list.Remove(node);
                    node.Data.Delete();
                    gjc_ta.Update(gjc_dt);
                }                            
            }       
        }

        

        private void btnJianqieKeyword_Click(object sender, RoutedEventArgs e)
        {
            node_jianqie=(Node)keywordTreeView.SelectedItem;

            MainWindow.mainWindow.statusBar.Items.Clear();
            TextBlock txtb = new TextBlock();
            txtb.Text = "已经剪切了关键词:ID="+ node_jianqie.Data.ID+"，关键词="+ node_jianqie.Data.关键词;
            MainWindow.mainWindow.statusBar.Items.Add(txtb);
        }

        private void btnNiantieKeyword_Click(object sender, RoutedEventArgs e)
        {
            Node node_father = get_father(node_jianqie);

            if (node_father != null)
            {
                node_father.Children.Remove(node_jianqie);

                Node node = (Node)keywordTreeView.SelectedItem;
                node.Children.Add(node_jianqie);

                //Node node2 = get_father(node);
                node.Children = new ObservableCollection<Node>(node.Children.OrderBy(item => item.Data.关键词));

                node_jianqie.Data.父节点 = node.Data.ID;
                gjc_ta.Update(gjc_dt);
            }
            else
            {
                roots.Remove(node_jianqie);
                Node node = (Node)keywordTreeView.SelectedItem;
                node.Children.Add(node_jianqie);

                if (get_father(node) != null)
                {
                    get_father(node).Children = new ObservableCollection<Node>(get_father(node).Children.OrderBy(item => item.Data.关键词));

                    node_jianqie.Data.父节点 = node.Data.ID;
                    gjc_ta.Update(gjc_dt);
                }
                else
                {
                    node_jianqie.Data.父节点 = node.Data.ID;
                    gjc_ta.Update(gjc_dt);
                    creat_tree();
                }
            }
        }

        /// <summary>
        /// 为该关键词增加一个单词
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddWord_Click(object sender, RoutedEventArgs e)
        {
            DataRow dr = dc_dt.NewRow();
            dr["单词"] = wordTextBox.Text;
            dc_dt.Rows.Add(dr);
            try
            {
                dc_ta.Update(dc_dt);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                dc_dt.Rows.Remove(dr);
                return;
            }

            MainWindow.mainWindow.statusBar.Items.Clear();
            TextBlock txtb = new TextBlock();
            txtb.Text = "新增单词：ID=" + newDataId + "，单词=" + wordTextBox.Text;
            MainWindow.mainWindow.statusBar.Items.Add(txtb);

            //为该单词添加关键词
            dr = dc_gjc_dt.NewRow();
            dr["单词ID"] = newDataId;
            Node node = (Node)keywordTreeView.SelectedItem;
            int keywordId = node.Data.ID;
            dr["关键词ID"] = keywordId;
            dc_gjc_dt.Rows.Add(dr);
            dc_gjc_ta.Update(dc_gjc_dt);
            
        }

        /// <summary>
        /// 为该关键词增加一个短语
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddDuanyu_Click(object sender, RoutedEventArgs e)
        {
            DataRow dr = dy_dt.NewRow();
            dr["短语"] = wordTextBox.Text;
            dy_dt.Rows.Add(dr);
            try
            {
                dy_ta.Update(dy_dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dy_dt.Rows.Remove(dr);
                return;
            }

            MainWindow.mainWindow.statusBar.Items.Clear();
            TextBlock txtb = new TextBlock();
            txtb.Text = "新增短语：ID=" + newDataId + "，短语=" + wordTextBox.Text;
            MainWindow.mainWindow.statusBar.Items.Add(txtb);

            //为该短语添加关键词
            dr = dy_gjc_dt.NewRow();
            dr["短语ID"] = newDataId;
            Node node = (Node)keywordTreeView.SelectedItem;
            int keywordId = node.Data.ID;
            dr["关键词ID"] = keywordId;
            dy_gjc_dt.Rows.Add(dr);
            dy_gjc_ta.Update(dy_gjc_dt);
        }
    }

    /// <summary>
    /// 节点，拥有通知属性
    /// </summary>
    public class Node: INotifyPropertyChanged
    {
        ScienceResearchDataSetNew.关键词Row data;
        ObservableCollection<Node> children;
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this,e);
        }

        public Node()
        {
            Children = new ObservableCollection<Node>();
        }

        public ScienceResearchDataSetNew.关键词Row Data
        {
            get { return data; }
            set
            {
                data = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Data"));
            }
        }

        public string Output
        {
            get
            {
                return Data.关键词;
            }
        }

        public ObservableCollection<Node> Children
        {
            get { return children; }
            set
            {
                children = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Children"));
            }
        }
    }
}
