var CategoryRow = React.createClass({
    deleteClicked: function (e) {
        e.preventDefault();
        this.props.delete(this.props.item.Id);
    },
    render: function () {
        return (
            <tr>
                <td></td>
                <td>{this.props.item.Name}</td>
                <td className="center">{this.props.item.Number}</td>
                <td>
                    <span className="{this.props.item.IsShow ? 'label label-success' : 'label label-danger'}">{this.props.item.IsShow ? 'Hiển thị' : 'Ẩn'}</span>
                </td>
                <td className="center">
                    <a href="#"><i className="fa fa-fw fa-edit"></i></a>
                    <a href="#" onClick={this.deleteClicked}><i className="fa fa-fw fa-trash"></i></a>
                </td>
            </tr>
            );
    }
});

var CategoryTable = React.createClass({
    loadData: function () {
        $.get(this.props.url, function (data) {
            this.setState({
                items: data
            });
        }.bind(this));
    },
    deleteNews: function (id) {
        var self = this;
        if (confirm("Bạn chắc chắn muốn xóa không?")) {
            $.post("/category/delete/" + id, function (data) {
                self.loadData();
            });
        }
    },
    getInitialState: function () {
        return {
            items: []
        }
    },
    componentDidMount: function () {
        console.log(this);
        this.loadData();
    },
    render: function () {
        //var rows = [];
        //this.state.items.forEach(function (item) {
        //    rows.push(<CategoryRow key={item.Id} item={item} onDeleteSubmit={this.handleDelete} />);
        //});

        var rows = this.state.items.map(function (item) {
            return (
             <CategoryRow key={item.Id} item={item} delete={this.deleteNews} />
                );
        }, this);

        return (
            <div className="box">
                <div className="box-body">
                    <div className="row">
                        <div className="col-md-12">
                            <button type="button" className="btn btn-primary"><i className="fa fa-plus-circle"></i>Thêm mới</button>
                            <button type="button" className="btn btn-danger"><i className="fa fa fa-trash"></i>Xóa</button>
                        </div>
                    </div>
                    <table className="table table-bordered">
                        <thead>
                            <tr>
                                <th className="center">#</th>
                                <th className="center">Tên loại sản phẩm</th>
                                <th className="center">TT hiển thị</th>
                                <th className="center">Trạng thái</th>
                                <th className="center">Chức năng</th>
                            </tr>
                        </thead>
                        <tbody>
                            {rows}
                        </tbody>
                    </table>
                </div>
                <div className="box-footer clearfix">
                        <ul className="pagination pagination-sm no-margin pull-right">
                            <li><a href="#">&laquo;</a></li>
                            <li><a href="#">1</a></li>
                            <li><a href="#">2</a></li>
                            <li><a href="#">3</a></li>
                            <li><a href="#">&raquo;</a></li>
                        </ul>
                    </div>
            </div>
            );
    }
});

ReactDOM.render(
    <CategoryTable url="/category/getall" />,
    document.getElementById('box')
    );